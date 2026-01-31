using ApplicationLayer.BankSystem.AbstractServices;
using Domainlayer.BankSystem.Entites;
using Domainlayer.BankSystem.Enums;
using Domainlayer.BankSystem.Requests;
using InfrastructureLayer.BankSystem.AbstractRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace ApplicationLayer.BankSystem.ImplementServices
{
    public class BankAccountService : IBankAccountService
    {
        #region Fields
        private IBankAccountRepository _accountRepository { get; set; }
        #endregion
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ILogger<BankAccountService> _logger;

        #region Constructor
        public BankAccountService(IBankAccountRepository BankAccountRepo, IHttpContextAccessor httpContextAccessor, ILogger<BankAccountService> logger)
        {
            _accountRepository = BankAccountRepo;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        #endregion
        #region Helpers (Extraction from Token)
        /// <summary>
        /// Gets the current authenticated user ID from JWT token
        /// </summary>
        private int CurrentUserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    _logger.LogWarning("Attempted to access CurrentUserId but user is not authenticated");
                    throw new UnauthorizedAccessException("User is not authenticated");
                }

                if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
                {
                    _logger.LogWarning("Invalid user ID in token: {UserIdClaim}", userIdClaim);
                    throw new UnauthorizedAccessException("Invalid user ID in token");
                }

                return userId;
            }
        }


        /// <summary>
        /// Checks if current user has Admin role
        /// </summary>
        private bool IsAdmin => _httpContextAccessor.HttpContext?.User?.IsInRole("Admin") ?? false;

        /// <summary>
        /// Gets current user's IP address for fraud detection
        /// </summary>
        private string CurrentIpAddress => _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "Unknown";
        #endregion

        public async Task<IEnumerable<BankAccount>> GetMyAccountsAsync()
        {
            _logger.LogInformation("GetMyAccountsAsync called by user {UserId}", CurrentUserId);

            var accounts = await _accountRepository.GetAccountsByUserIdAsync(CurrentUserId);
            return accounts.Where(a => !a.ISDeleted).ToList();
        }

        /// <summary>
        /// Get account by account number with authorization check
        /// </summary>
        public async Task<BankAccount> GetByAccountNumberAsync(string accountNumber)
        {
            _logger.LogInformation("GetByAccountNumberAsync called for account {AccountNumber} by user {UserId}",
                accountNumber, CurrentUserId);

            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                _logger.LogWarning("Empty account number provided");
                throw new ArgumentException("Account number is required", nameof(accountNumber));
            }

            var account = await _accountRepository.GetByAccountNumberAsync(accountNumber);

            if (account == null)
            {
                _logger.LogWarning("Account {AccountNumber} not found", accountNumber);
                throw new KeyNotFoundException($"Account with number {accountNumber} not found");
            }

            if (account.ISDeleted)
            {
                _logger.LogWarning("Account {AccountNumber} has been deleted", accountNumber);
                throw new KeyNotFoundException($"Account with number {accountNumber} not found");
            }

            // Authorization check
            if (!IsAdmin && account.UserId != CurrentUserId)
            {
                _logger.LogWarning("User {UserId} attempted to access account {AccountNumber} owned by user {OwnerId}",
                    CurrentUserId, accountNumber, account.UserId);
                throw new UnauthorizedAccessException("Access denied");
            }

            return account;
        }





        public async Task<IEnumerable<BankAccount>> GetAllAccountsAsync()
        {
            _logger.LogInformation("GetAllAccountsAsync called by user {UserId}, IsAdmin: {IsAdmin}",
                CurrentUserId, IsAdmin);

            // Admin only - regular users should use GetMyAccountsAsync
            if (!IsAdmin)
            {
                _logger.LogWarning("User {UserId} attempted to access GetAllAccountsAsync without admin privileges",
                    CurrentUserId);
                throw new UnauthorizedAccessException("Only administrators can view all accounts");
            }

            var accounts = await _accountRepository.GetAllAsync();
            return accounts.Where(a => !a.ISDeleted).ToList();
        }

        public async Task<BankAccount> GetAccountByIdAsync(int id)
        {
            _logger.LogInformation("GetAccountByIdAsync called for account {AccountId} by user {UserId}",
                id, CurrentUserId);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid account ID: {AccountId}", id);
                throw new ArgumentException("Invalid account ID", nameof(id));
            }

            var account = await _accountRepository.GetUserIdAsync(id);

            if (account == null)
            {
                _logger.LogWarning("Account {AccountId} not found", id);
                throw new KeyNotFoundException($"Account with ID {id} not found");
            }

            if (account.ISDeleted)
            {
                _logger.LogWarning("Account {AccountId} has been deleted", id);
                throw new KeyNotFoundException($"Account with ID {id} not found");
            }

            // Authorization: User can only access their own accounts unless they're admin
            if (!IsAdmin && account.UserId != CurrentUserId)
            {
                _logger.LogWarning("User {UserId} attempted to access account {AccountId} owned by user {OwnerId}",
                    CurrentUserId, id, account.UserId);
                throw new UnauthorizedAccessException("You don't have permission to access this account");
            }

            return account;
        }
        ///------------
        public async Task<BankAccount> CreateAccountAsync(CreateAccountRequest model)
        {
            _logger.LogInformation("CreateAccountAsync called by user {UserId}, Target: {TargetUserId}, Type: {AccountType}, Currency: {Currency}",
                CurrentUserId, model?.UserId, model?.AccountType, model?.Currency);

            // 1. Basic validation
            if (model == null)
            {
                _logger.LogWarning("CreateAccountAsync called with null model");
                throw new ArgumentNullException(nameof(model));
            }

            if (!Enum.IsDefined(typeof(AccountTypeEnum), model.AccountType))
            {
                _logger.LogWarning("Invalid account type: {AccountType}", model.AccountType);
                throw new ArgumentException("Invalid account type", nameof(model.AccountType));
            }

            if (!Enum.IsDefined(typeof(CurrencyEnum), model.Currency))
            {
                _logger.LogWarning("Invalid currency: {Currency}", model.Currency);
                throw new ArgumentException("Invalid currency", nameof(model.Currency));
            }

            // 2. Determine target user (Admin can create for anyone, regular user only for themselves)
            int targetUserId;

            if (IsAdmin)
            {
                // Admin can create account for any user
                targetUserId = model.UserId;

                if (targetUserId <= 0)
                {
                    _logger.LogWarning("Admin attempted to create account with invalid user ID: {UserId}", targetUserId);
                    throw new ArgumentException("Invalid user ID", nameof(model.UserId));
                }

                _logger.LogInformation("Admin {AdminId} creating account for user {TargetUserId}",
                    CurrentUserId, targetUserId);
            }
            else
            {
                // Regular user can only create account for themselves
                if (model.UserId != 0 && model.UserId != CurrentUserId)
                {
                    _logger.LogWarning("User {UserId} attempted to create account for user {TargetUserId}",
                        CurrentUserId, model.UserId);
                    throw new UnauthorizedAccessException("You can only create accounts for yourself");
                }
                targetUserId = CurrentUserId;
            }


            // 5. Prevent duplicate accounts (same type + currency)
            var existingAccounts = await _accountRepository.GetAccountsByUserIdAsync(targetUserId);
            var hasDuplicate = existingAccounts.Any(a =>
                a.AccountType == model.AccountType &&
                a.Currency == model.Currency &&
                !a.ISDeleted);

            if (hasDuplicate)
            {
                _logger.LogWarning("Duplicate account creation attempt: User {UserId}, Type: {AccountType}, Currency: {Currency}",
                    targetUserId, model.AccountType, model.Currency);
                throw new InvalidOperationException(
                    $"An active {model.AccountType} account with {model.Currency} currency already exists");
            }

            // 6. Generate unique account number
            var accountNumber = await GenerateUniqueAccountNumberAsync();

            // 7. Create account entity
            var newAccount = new BankAccount
            {
                UserId = targetUserId,
                AccountType = model.AccountType,
                Currency = model.Currency,
                AccountNumber = accountNumber,
                Balance = 0,
                IsActive = true,
                ISDeleted = false,
                CreatedData = DateTime.UtcNow,
                UpdatedData = DateTime.UtcNow
            };


            _logger.LogInformation("DEBUG DATABASE: Attempting to save account. Current UserId: {UserId}", newAccount.UserId);

            try
            {
                // التأكد من أن الـ UserId موجود فعلاً في السياق الحالي قبل الإرسال
                _logger.LogInformation("DEBUG DATA: Entity State before save - UserId: {UserId}, Account: {AccNo}",
                    newAccount.UserId, newAccount.AccountNumber);

                // 8. Save to database
                var result = await _accountRepository.AddAsync(newAccount);

                _logger.LogInformation("SUCCESS: Account saved with ID: {Id}", newAccount.Id);
                return result;
            }
            catch (Exception ex)
            {
                // طباعة تفاصيل الخطأ الداخلي (Inner Exception) لأنه هو اللي فيه السبب الحقيقي
                _logger.LogError("SQL ERROR DETECTED!");
                _logger.LogError("Message: {Msg}", ex.Message);
                _logger.LogError("Inner Exception: {Inner}", ex.InnerException?.Message);

                // إعادة الـ throw عشان الـ Handler اللي بره يمسكها
                throw;
            }

        }
        ///-------------------------------------
        public async Task<bool> UpdateAccountTypeAsync(int id, AccountTypeEnum newType)
        {
            _logger.LogInformation("UpdateAccountTypeAsync called for account {AccountId} by user {UserId}, New Type: {NewType}",
                id, CurrentUserId, newType);

            // Validate enum
            if (!Enum.IsDefined(typeof(AccountTypeEnum), newType))
            {
                _logger.LogWarning("Invalid account type: {AccountType}", newType);
                throw new ArgumentException("Invalid account type", nameof(newType));
            }

            // GetAccountByIdAsync already does authorization check
            var account = await GetAccountByIdAsync(id);

            // Check if account is active
            if (!account.IsActive)
            {
                _logger.LogWarning("Attempted to update inactive account {AccountId}", id);
                throw new InvalidOperationException("Cannot update an inactive account");
            }

            // Store old type for logging
            var oldType = account.AccountType;

            // Business Rule: Cannot switch between Business and Personal account types
            if ((oldType == AccountTypeEnum.Business && newType != AccountTypeEnum.Business) ||
                (oldType != AccountTypeEnum.Business && newType == AccountTypeEnum.Business))
            {
                _logger.LogWarning("Attempted to switch between Business/Personal for account {AccountId}: {OldType} → {NewType}",
                    id, oldType, newType);
                throw new InvalidOperationException(
                    "Cannot switch between Business and Personal account types. Please create a new account instead.");
            }

            // Check if same type
            if (oldType == newType)
            {
                _logger.LogInformation("Account type unchanged for account {AccountId}", id);
                return true; // No change needed
            }

            // Check for duplicate (same user, same new type, same currency)
            var userAccounts = await _accountRepository.GetAccountsByUserIdAsync(account.UserId);
            var hasDuplicate = userAccounts.Any(a =>
                a.Id != id &&
                a.AccountType == newType &&
                a.Currency == account.Currency &&
                !a.ISDeleted);

            if (hasDuplicate)
            {
                _logger.LogWarning("Update would create duplicate account: User {UserId}, Type: {NewType}, Currency: {Currency}",
                    account.UserId, newType, account.Currency);
                throw new InvalidOperationException(
                    $"User already has an active {newType} account with {account.Currency} currency");
            }

            // Update account
            account.AccountType = newType;
            account.UpdatedData = DateTime.UtcNow;

            await _accountRepository.UpdateAsync(account);

            _logger.LogInformation("Account {AccountNumber} type updated from {OldType} to {NewType} by user {UserId}",
                account.AccountNumber, oldType, newType, CurrentUserId);

            return true;
        }

        /// <summary>
        /// Soft delete account with validation
        /// </summary>
        public async Task<bool> DeleteAccountAsync(int id)
        {
            _logger.LogInformation("DeleteAccountAsync called for account {AccountId} by user {UserId}",
                id, CurrentUserId);

            // GetAccountByIdAsync already does authorization check
            var account = await GetAccountByIdAsync(id);

            // Check if already deleted
            if (account.ISDeleted)
            {
                _logger.LogWarning("Account {AccountId} is already deleted", id);
                throw new InvalidOperationException("Account is already deleted");
            }

            // Business Rule: Balance must be zero
            if (account.Balance != 0)
            {
                _logger.LogWarning("Attempted to delete account {AccountId} with non-zero balance: {Balance}",
                    id, account.Balance);
                throw new InvalidOperationException(
                    $"Account balance must be zero before deletion. Current balance: {account.Balance} {account.Currency}");
            }


            // Soft delete
            account.ISDeleted = true;
            account.IsActive = false;
            account.UpdatedData = DateTime.UtcNow;

            await _accountRepository.UpdateAsync(account);

            _logger.LogInformation("Account {AccountNumber} deleted by user {UserId}",
                account.AccountNumber, CurrentUserId);

            return true;
        }




        #region MethodsHandler

        /// <summary>
        /// Generate unique account number with collision detection
        /// Format: ACCT-{12-character-hex}
        /// </summary>
        private async Task<string> GenerateUniqueAccountNumberAsync()
        {
            const int maxAttempts = 10;
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                var guid = Guid.NewGuid().ToString("N"); // Remove hyphens
                var accountNumber = $"ACCT-{guid.Substring(0, 12).ToUpper()}";

                // Check if it exists
                var existing = await _accountRepository.GetByAccountNumberAsync(accountNumber);
                if (existing == null)
                {
                    _logger.LogDebug("Generated unique account number: {AccountNumber}", accountNumber);
                    return accountNumber;
                }

                attempts++;
                _logger.LogWarning("Account number collision detected: {AccountNumber}, attempt {Attempt}/{MaxAttempts}",
                    accountNumber, attempts, maxAttempts);
            }

            _logger.LogError("Failed to generate unique account number after {MaxAttempts} attempts", maxAttempts);
            throw new InvalidOperationException(
                $"Failed to generate unique account number after {maxAttempts} attempts. Please try again.");
        }


        public async Task<IEnumerable<BankAccount>> GetAccountsByUserIdAsync(int userId)
        {
            try
            {

                // 2. الحماية الأمنية (Security Check)
                // بنقرأ الـ UserId من الـ Token والـ Role
                var currentUserIdFromToken = CurrentUserId; // الـ Property اللي عملناها
                var isAdmin = IsAdmin;

                // "لو مش أدمن" وَ "الـ ID اللي مطلوب مش هو نفسه بتاع صاحب التوكن" -> ارفض فوراً
                if (!isAdmin && userId != currentUserIdFromToken)
                {
                    _logger.LogCritical("Security Breach Attempt: User {Attemptor} tried to access accounts of User {Target}",
                        currentUserIdFromToken, userId);

                    throw new UnauthorizedAccessException("Access Denied: You can only view your own accounts.");
                }

                // 3. التنفيذ (Execution) - لو عدى من الفلاتر اللي فوق
                _logger.LogInformation("Authorized access to accounts for UserId: {UserId}", userId);

                return await _accountRepository
                    .GetTableNoTracking()
                    .Where(acc => acc.UserId == userId && !acc.ISDeleted)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAccountsByUserIdAsync security layer");
                throw;
            }
        }

        #endregion
    }
}


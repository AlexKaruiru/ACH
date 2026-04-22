# Narration of Differences Between SQL Scripts

This document highlights the differences between the SQL scripts in the `Maendeleo LIVE` and `Maendeleo_MSD-0773` directories.

## 1. `p_AddInwardTrx.sql`
- **Logic Changes:**
  - Added logic to append `DrawerOrPayee` details to the transaction description based on `TrxTypeID`.
    ```sql
    IF ISNULL(@DrawerOrPayee, '') <> '' 
    BEGIN
        IF @TrxTypeID = 'IC'
            SET @TrxDescription = ISNULL(@TrxDescription, '') + ' | From: ' + LTRIM(RTRIM(@DrawerOrPayee))
        ELSE IF @TrxTypeID = 'ID'
            SET @TrxDescription = ISNULL(@TrxDescription, '') + ' | To: ' + LTRIM(RTRIM(@DrawerOrPayee))
    END
    ```
- **Structural Changes:**
  - [Describe any changes in structure, e.g., added/removed columns, parameters.]
- **Implementation Changes:**
  - [Describe any changes in implementation, e.g., new stored procedures, modified logic.]
- **Other Observations:**
  - [Any other notable differences.]

## 2. `p_AddOutwardTrx.sql`
- **Logic Changes:**
  - Similar logic as `p_AddInwardTrx.sql` to append `DrawerOrPayee` details to the transaction description.
- **Structural Changes:**
  - [Describe any changes in structure, e.g., added/removed columns, parameters.]
- **Implementation Changes:**
  - [Describe any changes in implementation, e.g., new stored procedures, modified logic.]
- **Other Observations:**
  - [Any other notable differences.]

## 3. `p_PostUploadFileData.sql`
- **Logic Changes:**
  - Added logic to stamp the originator (payer) account onto the clearing record for reporting.
    ```sql
    UPDATE t_TrxClearing
    SET    Reference              = @OriginatorAccountID
         ,AccountTypeID          = @OriginatorAccountTypeID  -- 'C' (Customer) not 'G' (GL)
    WHERE  TrxBatchID             = @TrxBatchID
      AND  TrxType                = 'OD'
      AND  DrawerOrPayeeAccountID = @CreditAccountID
      AND  ISNULL(Reference, '')  = ''
    ```
- **Structural Changes:**
  - [Describe any changes in structure, e.g., added/removed columns, parameters.]
- **Implementation Changes:**
  - [Describe any changes in implementation, e.g., new stored procedures, modified logic.]
- **Other Observations:**
  - [Any other notable differences.]

## 4. `v_Clearing.sql`
- **Logic Changes:**
  - Updated logic for `OD` (Outward EFT) rows to use the originator customer account stored in `Reference`.
    ```sql
    CASE
        WHEN Trxtype = 'OD' AND ISNULL(Reference, '') <> ''
            THEN AccountTypeID   -- already updated to 'C' by p_PostUploadFileData UPDATE
        ELSE AccountTypeID
    END AS AccountTypeID,
    CASE
        WHEN Trxtype = 'OD' AND ISNULL(Reference, '') <> ''
            THEN Reference       -- originator customer account (e.g. company salary payer)
        ELSE AccountID           -- GL account (CEN_BANK_AC) for all other cases
    END AS AccountID,
    ```
- **Structural Changes:**
  - [Describe any changes in structure, e.g., added/removed columns, parameters.]
- **Implementation Changes:**
  - [Describe any changes in implementation, e.g., new stored procedures, modified logic.]
- **Other Observations:**
  - [Any other notable differences.]

## Differences in `p_AddInwardTrx.sql`

### Parameter Definitions
- **"Maendeleo LIVE"**:
  - Includes parameters like `@ChequeDigit`, `@VoucherCode`, `@ReturnCodeID`, and `@VATPAYType`.
  - Default values are provided for optional fields.
- **"Maendeleo_MSD-0773"**:
  - Additional parameters such as `@SupervisedBy`, `@SupervisedOn`, and `@ForwardRemark`.
  - Some parameters have been renamed or adjusted for clarity.

### Logic Changes
- **"Maendeleo LIVE"**:
  - Simpler logic for handling `@ReturnCodeID` and `@VoucherCode`.
  - Less emphasis on supervision-related fields.
- **"Maendeleo_MSD-0773"**:
  - Introduces supervision logic with `@SupervisedBy` and `@SupervisedOn`.
  - Enhanced validation for `@ReturnCodeID` and `@VoucherCode`.
  - Additional checks for inter-branch transactions and supervision.

### Database Interactions
- **"Maendeleo LIVE"**:
  - Focuses on basic transaction insertion and validation.
  - Uses fewer stored procedures for auxiliary operations.
- **"Maendeleo_MSD-0773"**:
  - Includes calls to additional stored procedures like `p_ChargeTransaction`.
  - More detailed handling of inter-branch transactions and clearing.

### Error Handling
- **"Maendeleo LIVE"**:
  - Basic error handling with `RAISERROR` for common issues.
- **"Maendeleo_MSD-0773"**:
  - Enhanced error handling with specific error codes and messages.
  - Additional checks for supervision and inter-branch consistency.

### Comments and Formatting
- **"Maendeleo LIVE"**:
  - Sparse comments focusing on high-level explanations.
  - Less consistent formatting.
- **"Maendeleo_MSD-0773"**:
  - More detailed comments explaining logic and validations.
  - Improved formatting for readability.
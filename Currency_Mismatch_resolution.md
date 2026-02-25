# Currency Mismatch Resolution for Tanzania

## Overview

This document highlights the currency mapping inconsistencies identified in the Tanzania file generation process and the fixes implemented to standardize the behavior.

## Identified Problems

1.  **Inconsistent Integer Mappings:**
    - `BulkCheques` and `UnpaidCheques` typically used `TZS=0` and `USD=1`.
    - `SISTransaction` incorrectly mapped `TZS` to `1` and other currencies (like USD) to `2`.
    - `BulkCredit` lacked robust handling for string currency codes ("TZS", "USD") retrieved from some database contexts, causing potential conversion errors.
2.  **Enum Conversion Error (USD to Integer):**
    - In `BulkCheques`, the string "USD" was being incorrectly assigned to the `ActiveCurrencyCode` Enum property, triggering a "USD to Integer" conversion failure at runtime.
3.  **Missing Type Definitions:**
    - The project lacked definitions for `GenerationResult` and `MessageType`, leading to persistent compilation errors.

## UI Selection Bugs:

1.  **Hardcoded Currency:** The File Status form (`frmFileStatus.cs`) hardcoded the currency parameter to `0` (TZS) in all Tanzania file generation calls, ignoring the user's selection in the UI.
2.  **Broken Selection Logic:** The `cboFileType_SelectedIndexChanged` event used an incorrect switch comparison (`SelectedItem` as a boxed enum against `int`), causing the Bank list to fail to populate when changing file types.

## Implemented Fixes

### 1. Standardized Backend Mapping

Standardized all Tanzania-related currency mappings to **TZS = 0** and **USD = 1**.

**[TanzaniaFiles.vb](file:///C:/Users/alex.ndegwa/Desktop/Clearing/ACH/Common/Modules/TanzaniaFiles.vb)**

- **`SISTransaction`:** Updated mapping to `0` for TZS and `1` for others (USD).
- **`BulkCredit`:** Added support for both integer strings ("0", "1") and currency codes ("TZS", "USD") to improve robustness.
- **`BulkCheques` (Enum Fix):** Corrected the assignment of `ccy` (mapped integer) to the `ActiveCurrencyCode` Enum property, resolving the "USD" string conversion error.

### 2. Fixed UI Selection and Logic

**[frmFileStatus.cs](file:///C:/Users/alex.ndegwa/Desktop/Clearing/ACH/BRNETUploadDownload/frmFileStatus.cs)**

- **`btnCreate_Click`:** Replaced all hardcoded `0` currency parameters with `cboCurrency.SelectedIndex` to respect user choice.
- **`cboFileType_SelectedIndexChanged`:** Fixed the switch statement to use `SelectedIndex`, ensuring the Bank list populates correctly for all file types.
- **Default Selection:** Confirmed the UI correctly defaults to TZS (`SelectedIndex = 0`) on load.

### 3. Restored Missing Definitions

**[GenerationTypes.vb](file:///C:/Users/alex.ndegwa/Desktop/Clearing/ACH/Common/Modules/GenerationTypes.vb)**

- Created a new module to define `GenerationResult` and `MessageType`, resolving high-level compilation blockers and standardized error reporting.

## Verification

- Verified that selecting TZS in the UI now correctly passes `0` to the backend.
- Verified that selecting USD in the UI now correctly passes `1` to the backend.
- Confirmed the Bank list now updates dynamically when switching between Cheques, EFTs, and other instruments.

## Improved Error Handling and Feedback

To improve visibility into the file generation process, the following enhancements are being implemented:

1.  **Detailed Result Reporting:**
    - Transitioned from simple `Boolean` return values to a `GenerationResult` object that carries success status, summary messages, and a list of precise details (errors, warnings, and info messages).
2.  **Stored Procedure Error Capture:**
    - Explicitly capturing and surfacing database-level exceptions that occur during data retrieval or status updates.
3.  **Informative Messaging:**
    - Added success/info messages (e.g., "Generated 10 Cheques for Bank XYZ") to provide positive confirmation of work done.
4.  **Backend UI Decoupling:**
    - Removed `MessageBox.Show` calls from the backend modules, allowing the UI (`frmFileStatus.cs`) to control how errors are presented to the user.

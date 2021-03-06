package com.eazzyapps.example.android.ui.nav

import androidx.compose.runtime.Composable
import androidx.compose.ui.ExperimentalComposeUiApi
import androidx.navigation.NavHostController
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import com.eazzyapps.example.android.ui.composables.DeliveryNoteInputComposable
import com.eazzyapps.example.android.ui.composables.OptionSelectionUI
import com.eazzyapps.example.android.ui.composables.QuantityAdjustmentDialog
import com.eazzyapps.example.android.ui.composables.ScannerInputComposable

@ExperimentalComposeUiApi
@Composable
fun AppNavHost(controller: NavHostController) {
    NavHost(navController = controller, startDestination = Screen.NoteIdInput.route) {
        composable(Screen.NoteIdInput.route) { DeliveryNoteInputComposable() }
        composable(Screen.Scan.route) { ScannerInputComposable() }
        composable(Screen.BundleSelection.route) { OptionSelectionUI() }
        composable(Screen.QuantityAdjustment.route) { QuantityAdjustmentDialog() }

    }
}
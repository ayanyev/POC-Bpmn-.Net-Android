package com.eazzyapps.example.android.ui.nav

import android.os.Parcelable
import androidx.navigation.NavController
import androidx.navigation.NavOptionsBuilder
import androidx.navigation.compose.navigate

//TODO think on how to provide routes with arguments in more
// generic way without copypasting
sealed class Screen(val route: String, val popBackStack : Boolean = false) {
    object Previous : Screen("back")
    object NoteIdInput : Screen("noteIdInputScreen", true)
    object Scan : Screen("scanInputScreen", true)
    object BundleSelection : Screen("bundleSelectionScreen", true)
    object QuantityAdjustment : Screen("quantityAdjustmentScreen", true)
}

fun NavController.navigate(
    route: String,
    parcelableArgs: Map<String, Parcelable> = mapOf(),
    builder: NavOptionsBuilder.() -> Unit = {}
) {
    currentBackStackEntry?.arguments?.apply {
        for (arg in parcelableArgs) {
            putParcelable(arg.key, arg.value)
        }
    } ?: if (parcelableArgs.isNotEmpty()) error("Cannot set arguments!!!")
    navigate(route, builder)
}
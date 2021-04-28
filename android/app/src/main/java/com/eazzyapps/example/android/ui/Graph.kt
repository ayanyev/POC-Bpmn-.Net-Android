package com.eazzyapps.example.android.ui

import androidx.compose.runtime.Composable
import androidx.compose.ui.ExperimentalComposeUiApi
import androidx.navigation.NavHostController
import androidx.navigation.compose.NavHost
import com.eazzyapps.example.android.Screen

@ExperimentalComposeUiApi
@Composable
fun AppNavHost(controller: NavHostController) {
    NavHost(navController = controller, startDestination = Screen.Main.route) {
/*
        composable(Screen.Main.route) { MainContent() }
*/
    }
}
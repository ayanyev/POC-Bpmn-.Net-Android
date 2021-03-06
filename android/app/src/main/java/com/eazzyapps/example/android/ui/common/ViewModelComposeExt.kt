package com.eazzyapps.example.android

import androidx.compose.runtime.Composable
import androidx.compose.runtime.remember
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewmodel.compose.LocalViewModelStoreOwner
import org.koin.androidx.viewmodel.ViewModelOwner
import org.koin.androidx.viewmodel.koin.getViewModel
import org.koin.core.Koin
import org.koin.core.context.GlobalContext
import org.koin.core.parameter.ParametersDefinition
import org.koin.core.qualifier.Qualifier

// TODO: workaround until this is fixed https://github.com/InsertKoinIO/koin/issues/1006
@Composable
inline fun <reified T : ViewModel> getViewModel(
    qualifier: Qualifier? = null,
    noinline parameters: ParametersDefinition? = null,
): T {
    val owner = LocalViewModelStoreOwner.current!!
    return remember {
        GlobalContext.get().getViewModel(
            qualifier,
            owner = { ViewModelOwner.from(owner) },
            parameters = parameters,
        )
    }
}

@Composable
inline fun <reified T> get(
    qualifier: Qualifier? = null,
    noinline parameters: ParametersDefinition? = null
): T = remember {
    GlobalContext.get().get(qualifier, parameters)
}
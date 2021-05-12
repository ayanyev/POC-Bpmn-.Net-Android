package com.eazzyapps.example.android

import com.eazzyapps.example.android.ui.common.ActivityDelegate
import com.eazzyapps.example.android.ui.viewmodel.DexScannerViewModel
import com.eazzyapps.example.android.ui.viewmodel.IntakeViewModel
import com.eazzyapps.example.android.ui.viewmodel.QuantityAdjustmentViewModel
import org.koin.androidx.viewmodel.dsl.viewModel
import org.koin.dsl.module

val uiModule = module {

    viewModel {(onItemScanned : (String) -> Unit) ->
        DexScannerViewModel(
            doOnItemScanned = onItemScanned,
            scanner = get()
        )
    }

    single {
        // TODO keep an eye on if delegate singleton works fine
        //  if there are more then one Activity in app
        ActivityDelegate()
    }

    single {
        IntakeViewModel()
    }

    viewModel {
        QuantityAdjustmentViewModel()
    }

}
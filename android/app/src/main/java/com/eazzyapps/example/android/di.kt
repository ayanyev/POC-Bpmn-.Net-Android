package com.eazzyapps.example.android


import com.eazzyapps.example.android.ui.viewmodel.DexScannerViewModel
import org.koin.androidx.viewmodel.dsl.viewModel
import org.koin.dsl.module

val uiModule = module {

    viewModel {
        DexScannerViewModel(get())
    }

}
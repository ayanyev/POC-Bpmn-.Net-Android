package com.eazzyapps.example.android


import com.eazzyapps.example.android.ui.viewmodel.DexScannerViewModel
import org.koin.androidx.viewmodel.dsl.viewModel
import org.koin.dsl.module

val uiModule = module {

    single {
        // TODO keep an eye on if delegate singleton works fine
        //  if there are more then one Activity in app
        ActivityDelegate()
    }
    viewModel {
        DexScannerViewModel(get())
    }

}
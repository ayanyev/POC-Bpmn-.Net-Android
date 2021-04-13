package com.eazzyapps.example.android

import android.app.Application
import eu.durstexpress.modules.scanner.scannerModule_Module
import org.koin.android.ext.koin.androidContext
import org.koin.android.ext.koin.androidLogger
import org.koin.core.context.startKoin

class DemoApp : Application() {

    override fun onCreate() {
        super.onCreate()
        initKoin()
    }

    private fun initKoin() {
        startKoin {
            androidLogger()
            androidContext(this@DemoApp)
            modules(
                scannerModule_Module,
                uiModule
            )
        }
    }

}
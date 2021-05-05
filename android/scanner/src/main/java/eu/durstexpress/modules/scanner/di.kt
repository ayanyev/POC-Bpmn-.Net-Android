package eu.durstexpress.modules.scanner

import org.koin.dsl.module
import org.koin.experimental.builder.singleBy


val scannerModule_Module = module(createdAtStart = true) {

    singleBy<Scanner, ZebraScanner>()

}
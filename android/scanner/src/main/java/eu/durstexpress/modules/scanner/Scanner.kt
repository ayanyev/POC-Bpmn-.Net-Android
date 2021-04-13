package eu.durstexpress.modules.scanner

import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.Observer

interface Scanner {

    fun enable()

    fun disable()

    fun observe(owner: LifecycleOwner, observer: Observer<String>)

    suspend fun observe(observer: Observer<String>)

    fun emitResult(result: String)
}
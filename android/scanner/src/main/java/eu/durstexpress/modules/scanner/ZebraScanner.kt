package eu.durstexpress.modules.scanner

import android.content.BroadcastReceiver
import android.content.Context
import android.content.Intent
import android.content.IntentFilter
import android.os.Bundle
import androidx.lifecycle.LifecycleOwner
import androidx.lifecycle.Observer
import kotlinx.coroutines.*
import kotlinx.coroutines.flow.*
import timber.log.Timber
import kotlin.coroutines.resume

class ZebraScanner(

    private val context: Context

) : Scanner, CoroutineScope {

    private val handler = CoroutineExceptionHandler { _, exception ->
        print(exception.message)
    }

    override val coroutineContext by lazy {
        CoroutineScope(Dispatchers.Default + SupervisorJob() + handler).coroutineContext
    }

    companion object {

        const val DW_ACTION = "com.symbol.datawedge.api.ACTION"
        const val DW_RESULT_ACTION = "com.symbol.datawedge.api.RESULT_ACTION"
        const val DW_CATEGORY = "android.intent.category.DEFAULT"

        const val DW_GET_PROFILES_RESULT_KEY = "com.symbol.datawedge.api.RESULT_GET_PROFILES_LIST"
        const val DW_RESULT_KEY = "com.symbol.datawedge.data_string"

        const val DW_GET_PROFILES_COMMAND = "com.symbol.datawedge.api.GET_PROFILES_LIST"

        const val DW_SET_CONFIG_COMMAND = "com.symbol.datawedge.api.SET_CONFIG"
        const val DW_SET_CONFIG_CID = "set/update profile"

        const val DW_SWITCH_TO_PROFILE_COMMAND = "com.symbol.datawedge.api.SWITCH_TO_PROFILE"
        const val DW_SWITCH_TO_PROFILE_CID = "switch to profile"

        const val DISABLED_PROFILE_NAME = "dex_disabled"

    }

    private val dexScanAction: String get() = context.packageName

    private val enabledProfileName: String get() = context.resources.getString(context.applicationInfo.labelRes)

    private val scanLiveData = SingleLiveEvent<String>()

    private val scanFlow = MutableStateFlow("")

    init {
        subscribeForScanResults()
    }

    private var isProfilesCreated = false

    private var activeCount = 0

    override fun enable() = doEnable()

    override fun disable() = doDisable()

    override fun observe(owner: LifecycleOwner, observer: Observer<String>) {
        scanLiveData.observe(owner, observer)
    }

    override suspend fun observe(observer: Observer<String>) {
        scanFlow.filter { it.isNotEmpty() }.collect { observer.onChanged(it) }
    }

    override fun emitResult(result: String) {
        scanFlow.value = result
    }

    private fun doDisable() {
        activeCount--
        launch {
            doSwitchToProfile(DISABLED_PROFILE_NAME)
        }
    }

    private fun doEnable() {
        activeCount++
        launch {
            if (isProfilesCreated || doCreateProfilesAsync().await()) {
                doSwitchToProfile(enabledProfileName)
            }
        }
    }

    private fun doSwitchToProfile(name: String) {
        if (activeCount > 0 && name == DISABLED_PROFILE_NAME) return

        launch {
            context.switchToProfile(name).also { isSuccessful ->
                if (!isSuccessful) {
                    throw Exception("Scanner profile is not available")
                }
            }
        }
    }

    private fun doCreateProfilesAsync() = async {
        with(context) {
            createProfile(enabledProfileName, true)
                    && createProfile(DISABLED_PROFILE_NAME, false).also { created ->
                isProfilesCreated = created
            }
        }
    }

    private suspend fun Context.createProfileIntent(name: String, enabled: Boolean): Intent {

        val isEnabled = when (enabled) {
            true -> "true"
            false -> "false"
        }

        val configMode = when (isProfileExistent(name)) {
            true -> "OVERWRITE"
            false -> "CREATE_IF_NOT_EXIST"
        }

        // Intent output
        val intentOutputBundle = Bundle().apply {
            putString("intent_output_enabled", "true")
            putString(
                "intent_action",
                dexScanAction
            )
            putString(
                "intent_category",
                DW_CATEGORY
            )
            putInt("intent_delivery", 2) // broadcast
        }

        // PARAMS_LIST
        val paramsListBundle = Bundle().apply {
            putString("current-device-id", "0")
            putString("scanner_input_enabled", "true")
            putAll(intentOutputBundle)
        }

        // PLUGIN_CONFIG
        val pluginConfigBundle = Bundle().apply {
            putString("PLUGIN_NAME", "INTENT")
            putString("RESET_CONFIG", "true")
            putBundle("PARAM_LIST", paramsListBundle)
        }

        // APP_LIST
//        val appsListBundle = Bundle().apply {
//            putString("PACKAGE_NAME", context.packageName)
//            putStringArray("ACTIVITY_LIST", arrayOf("*"))
//        }

        // MAIN
        val profileBundle = Bundle().apply {
            putString("PROFILE_NAME", name)
            putString("PROFILE_ENABLED", isEnabled)
            putString("CONFIG_MODE", configMode)
            putBundle("PLUGIN_CONFIG", pluginConfigBundle)
//            putParcelableArray("APP_LIST", arrayOf(appsListBundle))
        }

        return Intent().apply {
            action =
                DW_ACTION
            putExtra(DW_SET_CONFIG_COMMAND, profileBundle)
            putExtra("SEND_RESULT", "true")
            putExtra(
                "COMMAND_IDENTIFIER",
                DW_SET_CONFIG_CID
            )
        }

    }

    private fun subscribeForScanResults() {

        val filter = IntentFilter().apply {
            addAction(dexScanAction)
            addCategory(DW_CATEGORY)
        }

        val receiver = object : BroadcastReceiver() {

            override fun onReceive(context: Context?, intent: Intent?) {

                when (intent!!.action) {

                    dexScanAction -> {

                        val result = intent.getStringExtra(DW_RESULT_KEY) ?: "Empty"
                        scanFlow.value = result
/*
                        liveData.postValue(result)
*/
                        Timber.d("Scan result $result")

                    }
                }
            }
        }

        context.registerReceiver(receiver, filter)
        Timber.d("Subscribed to scan results")

    }

    private suspend fun Context.createProfile(name: String, enabled: Boolean): Boolean {

        val intent = createProfileIntent(name, enabled)
        Timber.d("Profile($name) intent created")

        sendBroadcast(intent)

        return suspendCancellableCoroutine { continuation ->

            val receiver = object : BroadcastReceiver() {
                override fun onReceive(context: Context?, intent: Intent?) {
                    when (intent!!.action) {
                        DW_RESULT_ACTION -> {
                            intent.extractResultForCommandId(DW_SET_CONFIG_CID) { isSuccessful ->
                                unregisterReceiver(this)
                                continuation.resume(isSuccessful)
                            }
                        }
                    }
                }
            }

            continuation.invokeOnCancellation { unregisterReceiver(receiver) }

            registerReceiver(receiver, IntentFilter().apply {
                addAction(DW_RESULT_ACTION)
                addCategory(DW_CATEGORY)
            })
        }

    }

    private suspend fun Context.isProfileExistent(profileName: String): Boolean =
        suspendCancellableCoroutine { continuation ->

            fun getProfilesList() = sendBroadcast(Intent().apply {
                action =
                    DW_ACTION
                putExtra(DW_GET_PROFILES_COMMAND, "")
            })

            val receiver = object : BroadcastReceiver() {
                override fun onReceive(context: Context?, intent: Intent?) {
                    when (intent!!.action) {
                        DW_RESULT_ACTION -> {
                            if (intent.hasExtra(DW_GET_PROFILES_RESULT_KEY)) {
                                unregisterReceiver(this)
                                val isExistent = intent.getStringArrayExtra(
                                    DW_GET_PROFILES_RESULT_KEY
                                )?.contains(profileName)
                                    ?: false
                                Timber.d("Profile $profileName ${if (isExistent) "" else "NOT"} exists")
                                continuation.resume(isExistent)
                            }
                        }
                    }
                }
            }

            continuation.invokeOnCancellation { unregisterReceiver(receiver) }

            registerReceiver(receiver, IntentFilter().apply {
                addAction(DW_RESULT_ACTION)
                addCategory(DW_CATEGORY)
            })

            getProfilesList()

        }

    private suspend fun Context.switchToProfile(name: String): Boolean =
        suspendCancellableCoroutine { continuation ->

            val receiver = object : BroadcastReceiver() {
                override fun onReceive(context: Context?, intent: Intent?) {
                    when (intent!!.action) {
                        DW_RESULT_ACTION -> {
                            intent.extractResultForCommandId(DW_SWITCH_TO_PROFILE_CID) { isSuccessful ->
                                unregisterReceiver(this)
                                continuation.resume(isSuccessful)
                            }
                        }
                    }
                }
            }

            continuation.invokeOnCancellation { unregisterReceiver(receiver) }

            registerReceiver(receiver, IntentFilter().apply {
                addAction(DW_RESULT_ACTION)
                addCategory(DW_CATEGORY)
            })

            Intent().apply {
                action = "com.symbol.datawedge.api.ACTION"
                putExtra(DW_SWITCH_TO_PROFILE_COMMAND, name)
                putExtra(
                    "SEND_RESULT",
                    "true"
                ) // from DataWedge 7.1 might be necessary to use "LAST_RESULT"
                putExtra(
                    "COMMAND_IDENTIFIER",
                    DW_SWITCH_TO_PROFILE_CID
                )
                context.sendBroadcast(this)
            }

        }

    private fun Intent.extractResultForCommandId(cid: String, action: (Boolean) -> Unit = {}) {

        val commandId = getStringExtra("COMMAND_IDENTIFIER")

        if (commandId == cid) {

            val command = getStringExtra("COMMAND")
            val result = getStringExtra("RESULT")
            val resultInfo = getBundleExtra("RESULT_INFO")?.run {
                keySet().map { "$it : ${getString(it)}" }.joinToString("\n\t\t\t\t\t\t\t")
            } ?: "no info"

            action(result == "SUCCESS")

            Timber.d("==>\n\t\t\t\tCommand: $command\n\t\t\t\tResult: $result\n\t\t\t\tResult info: $resultInfo\n\t\t\t\tCID: $commandId")
        }

    }

}



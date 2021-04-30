package com.eazzyapps.example.android

import android.os.Bundle
import androidx.activity.compose.setContent
import androidx.appcompat.app.AppCompatActivity
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.CornerSize
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.*
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Menu
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.ExperimentalComposeUiApi
import androidx.compose.ui.Modifier
import androidx.compose.ui.geometry.Size
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalDensity
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.LayoutDirection
import androidx.compose.ui.unit.dp
import androidx.lifecycle.lifecycleScope
import androidx.lifecycle.viewmodel.compose.viewModel
import androidx.navigation.compose.rememberNavController
import com.eazzyapps.example.android.domain.ScanEvent
import com.eazzyapps.example.android.domain.SelectionOptions
import com.eazzyapps.example.android.domain.TaskCategory.*
import com.eazzyapps.example.android.domain.ValidBarcodes
import com.eazzyapps.example.android.ui.*
import com.eazzyapps.example.android.ui.composables.StartAsLayout
import com.eazzyapps.example.android.ui.theme.AndroidTheme
import com.eazzyapps.example.android.ui.theme.grey
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.InternalCoroutinesApi
import kotlinx.coroutines.flow.collect
import kotlinx.coroutines.flow.merge
import kotlinx.coroutines.launch
import org.koin.android.ext.android.inject
import java.util.*

val space = 16.dp

class IntakeActivity : AppCompatActivity() {

    private val delegate by inject<ActivityDelegate>()

    @ExperimentalComposeUiApi
    @InternalCoroutinesApi
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {

            val isLoading by delegate.loadingFlow.collectAsState()

            val controller = rememberNavController()

            val scaffoldState = rememberScaffoldState(rememberDrawerState(DrawerValue.Closed))

            val scope = remember { lifecycleScope }

            scope.launchWhenResumed {
                merge(delegate.msgFlow, delegate.navFlow).collect{
                    when (it) {
                        is Message -> launch {
                            // launch in another coroutine
                            // to avoid waiting showSnackbar returning result
                            scaffoldState.snackbarHostState.apply {
                                // dismiss current snackbar first. if any
                                currentSnackbarData?.dismiss()
                                showSnackbar(it.getText(this@IntakeActivity))
                            }
                        }
                        is Screen -> {
                            // dismiss current snackbar before navigate
                            scaffoldState.snackbarHostState.currentSnackbarData?.dismiss()
                            if (it is Screen.Previous) controller.navigateUp()
                            else controller.navigate(it.route) {
                                if(it.popBackStack){
                                    controller.popBackStack()
                                }
                            }
                        }
                    }
                }
            }

            AndroidTheme() {
                // A surface container using the 'background' color from the theme
                Surface {
                    Box(modifier = Modifier.fillMaxSize()) {

                        val viewModel: IntakeViewModel = viewModel()

                        val isReadyToConnect by viewModel.isLoggedIn.collectAsState()

                        val isConnected by viewModel.isConnected.collectAsState()

                        val isRunning by viewModel.isProcessRunning.collectAsState()

                        val processName by viewModel.screenTitle.collectAsState()

                        LaunchedEffect(key1 = isRunning) {
                            if (isRunning) scaffoldState.drawerState.close()
                            else scaffoldState.drawerState.open()
                        }

                        Scaffold(
                            scaffoldState = scaffoldState,
                            topBar = {
                                TabBarLayout(
                                    title = processName,
                                    scope = rememberCoroutineScope(),
                                    scaffoldState = scaffoldState
                                )
                            },
                            drawerContent = {
                                DrawerLayout(
                                    isConnected = isConnected,
                                    isReadyToConnect = isReadyToConnect,
                                    isRunning = isRunning,
                                    viewModel = viewModel
                                )
                            },
                            content = {
                                MainContent(
                                    isRunning = isRunning,
                                    viewModel = viewModel
                                )
                            }
                        )

                        ProgressIndicator(isLoading)
                    }
                }
            }
        }
    }
}

@Composable
fun ProgressIndicator(isLoading: Boolean) {
    if (!isLoading) return
    Box(
        modifier = Modifier
            .fillMaxSize()
            .background(Color.Black.copy(alpha = 0.2f))
    ) {
        CircularProgressIndicator(
            modifier = Modifier.align(Alignment.Center)
        )
    }
}

@OptIn(ExperimentalStdlibApi::class)
@Composable
fun TabBarLayout(
    title: String,
    scope: CoroutineScope,
    scaffoldState: ScaffoldState
) {
    TopAppBar(
        title = { Text(text = title.uppercase(Locale.getDefault())) },
        backgroundColor = MaterialTheme.colors.surface,
        navigationIcon = {
            Icon(
                imageVector = Icons.Default.Menu,
                contentDescription = "",
                modifier = Modifier
                    .padding(start = 16.dp)
                    .clickable(onClick = {
                        scope.launch {
                            scaffoldState.drawerState.open()
                        }
                    })
            )
        }
    )
}

@OptIn(ExperimentalStdlibApi::class)
@Composable
fun DrawerLayout(
    isConnected: Boolean,
    isReadyToConnect: Boolean,
    isRunning: Boolean,
    viewModel: IntakeViewModel
) {

    Column(
        modifier = Modifier
            .fillMaxHeight()
            .padding(48.dp),
        verticalArrangement = Arrangement.Top,
        horizontalAlignment = Alignment.CenterHorizontally
    ) {

        StartAsLayout(
            items = viewModel.names,
            selectionEnabled = !isConnected,
            onSelected = { index -> viewModel.setLogIn(index) }
        )

        if (isReadyToConnect) {

            Spacer(modifier = Modifier.height(space))

            OutlinedButton(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                onClick = { viewModel.connect() },
                enabled = !isConnected,
            ) {
                Text(
                    textAlign = TextAlign.Center,
                    text = "Log in".uppercase(Locale.getDefault())
                )
            }

            Spacer(modifier = Modifier.height(space))

            OutlinedButton(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                onClick = {
                    viewModel.startProcess()
                          },
                enabled = isConnected && !isRunning
            ) {
                Text(
                    textAlign = TextAlign.Center,
                    text = "Start process".uppercase(Locale.getDefault())
                )
            }

            Spacer(modifier = Modifier.height(space))

            OutlinedButton(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                onClick = { viewModel.stopProcess() },
                enabled = isConnected && isRunning
            ) {
                Text(
                    textAlign = TextAlign.Center,
                    text = "Stop process".uppercase(Locale.getDefault())
                )
            }

            Spacer(modifier = Modifier.height(space))

            OutlinedButton(
                modifier = Modifier
                    .wrapContentHeight()
                    .fillMaxWidth(),
                onClick = { viewModel.disconnect() },
                enabled = isConnected
            ) {
                Text(
                    textAlign = TextAlign.Center,
                    text = "Log out".uppercase(Locale.getDefault())
                )
            }
        }
    }

}

@ExperimentalComposeUiApi
@Composable
fun MainContent(
    isRunning: Boolean,
    viewModel: IntakeViewModel
) {
    if (isRunning) {

        val currentTask by viewModel.currentTask.collectAsState()

        val isConnected by viewModel.isConnected.collectAsState()

        val articles by viewModel.articleList.collectAsState()

        val isInputError by viewModel.isError.collectAsState()

        val bottomSheetState = rememberBottomSheetState(BottomSheetValue.Collapsed)

        val bottomSheetScaffoldState = rememberBottomSheetScaffoldState(
            bottomSheetState = bottomSheetState
        )

        BottomSheetScaffold(
            scaffoldState = bottomSheetScaffoldState,
            sheetShape = RoundedCornerShape(20.dp),
            sheetContent = {
                ArticlesListHeader(articles.count(), bottomSheetState.isCollapsed)
                Divider(color = grey.copy(alpha = 0.3f), thickness = 1.dp)
                ArticleListComposable(articles = articles)
            },
            sheetPeekHeight = if (articles.isNotEmpty()) 64.dp else 0.dp
        ) {

            when (currentTask.category) {
                is Simple -> {
                    DeliveryNoteInputComposable(
                        onInputSubmit = {
                            viewModel.sendInputData(currentTask.toResult(it))
                        },
                        isError = currentTask.hasError
                    )
                }
                is Scan -> {

                    val scanType = ScanEvent.of(currentTask.id)

                    val availableBarcodes = (currentTask.payload as? ValidBarcodes)?.barcodes

                    ScanInfoComposable(
                        title = scanType.label,
                        availableBarcodes = availableBarcodes
                    )
                    ScannerInputComposable(
                        hint = currentTask.label,
                        isError = isInputError,
                        errorMsg = scanType.errorMsg,
                        onItemScanned = {
                            if (it.isNotBlank()) {
                                if (availableBarcodes == null || availableBarcodes.contains(it)) {
                                    viewModel.apply {
                                        selectScannedItem(it)
                                        setInputError(false)
                                        sendInputData(currentTask.toResult(it))
                                    }
                                } else if (it.isNotEmpty()) {
                                    viewModel.setInputError(true)
                                }
                            }
                        }
                    )
                }

                is Selection -> {
                    OptionSelectionUI(
                        optionList = (currentTask.payload as SelectionOptions).items,
                        doOnConfirm = {
                            viewModel.sendInputData(currentTask.toResult(it))
                        },
                        isError = currentTask.error != null,
                        errorMsg = currentTask.error?.message ?: ""
                    )
                }

                is Quantity, is AdjustQuantity -> {
                    QuantityAdjustmentDialog(
                        title = currentTask.label,
                        doOnConfirm = {
                            viewModel.sendInputData(currentTask.toResult(it))
                        }
                    )
                }
                else -> {}
            }

        }
    }
}
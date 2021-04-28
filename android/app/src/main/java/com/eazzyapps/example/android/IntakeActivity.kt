package com.eazzyapps.example.android

import android.os.Bundle
import androidx.activity.compose.setContent
import androidx.appcompat.app.AppCompatActivity
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.material.*
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Menu
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.lifecycle.viewmodel.compose.viewModel
import com.eazzyapps.example.android.domain.TaskCategory.*
import com.eazzyapps.example.android.ui.composables.StartAsLayout
import com.eazzyapps.example.android.ui.composables.TestLayout
import com.eazzyapps.example.android.ui.theme.AndroidTheme
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.launch
import java.util.*

val space = 16.dp

class IntakeActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            AndroidTheme(darkTheme = true) {

                val viewModel: IntakeViewModel = viewModel()

                val isReadyToConnect by viewModel.isLoggedIn.collectAsState()

                val isConnected by viewModel.isConnected.collectAsState()

                val isRunning by viewModel.isProcessRunning.collectAsState()

                val processName by viewModel.screenTitle.collectAsState()

                val scaffoldState = rememberScaffoldState(rememberDrawerState(DrawerValue.Closed))

                val scope = rememberCoroutineScope()

                LaunchedEffect(key1 = isRunning) {
                    if (isRunning) scaffoldState.drawerState.close()
                    else scaffoldState.drawerState.open()
                }

                Scaffold(
                    scaffoldState = scaffoldState,
                    topBar = {
                        TabBarLayout(
                            title = processName,
                            scope = scope,
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
            }
        }
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
                onClick = { viewModel.startProcess() },
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

@Composable
fun MainContent(
    isRunning: Boolean,
    viewModel: IntakeViewModel
) {
    if (isRunning) {

        val currentTask by viewModel.currentTask.collectAsState()

        Box(
            modifier = Modifier.fillMaxSize(),
            contentAlignment = Alignment.Center
        ) {

            Column(
                modifier = Modifier
                    .fillMaxHeight()
                    .width(250.dp),
                verticalArrangement = Arrangement.Center,
                horizontalAlignment = Alignment.CenterHorizontally
            ) {

                Spacer(modifier = Modifier.height(space))

                TestLayout(
                    modifier = Modifier
                        .wrapContentHeight()
                        .fillMaxWidth(),
                    initial = "note1",
                    label = if (currentTask.category is Simple) currentTask.label else "",
                    isEnabled = isRunning && currentTask.category is Simple,
                    isError = currentTask.hasError,
                    onClick = {
                        viewModel.sendInputData(currentTask.toResult(it))
                    }
                )

                Spacer(modifier = Modifier.height(space))

                TestLayout(
                    modifier = Modifier
                        .wrapContentHeight()
                        .fillMaxWidth(),
                    initial = "567.456.7.9",
                    label = if (currentTask.category is Scan) currentTask.label else "",
                    isEnabled = isRunning && currentTask.category is Scan,
                    isError = currentTask.hasError,
                    onClick = {
                        viewModel.sendInputData(currentTask.toResult(it))
                    }
                )

                Spacer(modifier = Modifier.height(space))

                TestLayout(
                    modifier = Modifier
                        .wrapContentHeight()
                        .fillMaxWidth(),
                    initial = "1",
                    label = if (currentTask.category is Selection) currentTask.label else "",
                    isEnabled = isRunning && currentTask.category is Selection,
                    isError = currentTask.hasError,
                    onClick = {
                        viewModel.sendInputData(currentTask.toResult(it.toInt()))
                    }
                )

                Spacer(modifier = Modifier.height(space))

                TestLayout(
                    modifier = Modifier
                        .wrapContentHeight()
                        .fillMaxWidth(),
                    initial = "25",
                    label = if (currentTask.category is AdjustQuantity || currentTask.category is Quantity) currentTask.label else "",
                    isEnabled = isRunning && ((currentTask.category is Quantity) || (currentTask.category is AdjustQuantity)),
                    isError = currentTask.hasError,
                    onClick = {
                        viewModel.sendInputData(currentTask.toResult(it.toInt()))
                    }
                )
            }
        }
    }
}
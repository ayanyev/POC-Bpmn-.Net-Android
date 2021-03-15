package com.eazzyapps.example.android

import android.os.Bundle
import androidx.activity.compose.setContent
import androidx.appcompat.app.AppCompatActivity
import androidx.compose.foundation.layout.*
import androidx.compose.material.MaterialTheme
import androidx.compose.material.OutlinedButton
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.lifecycle.viewmodel.compose.viewModel
import com.eazzyapps.example.android.ui.theme.AndroidTheme

class MainActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent { ScreenUi() }
    }
}

@Composable
fun ScreenUi() {

    val viewModel: SignalRViewModel = viewModel()

    val isOnShift by viewModel.isOnShift.collectAsState()

    val isTaskAssigned by viewModel.hasTask.collectAsState()

    val taskUUID by viewModel.task.collectAsState()

    Box(modifier = Modifier.fillMaxSize()) {
        ShiftToggle(
            modifier = Modifier.align(Alignment.Center),
            name = viewModel.name,
            isOnShift = isOnShift,
            hasTask = isTaskAssigned,
            taskId = taskUUID,
            onStart = { viewModel.startShift() },
            onStop = { viewModel.stopShift() },
            onDone = { viewModel.confirmTaskDone() }
        )
    }

}

@Composable
fun ShiftToggle(
    modifier: Modifier,
    name: String,
    taskId: String,
    isOnShift: Boolean,
    hasTask: Boolean,
    onStart: () -> Unit,
    onStop: () -> Unit,
    onDone: () -> Unit
) {
    Column(
        modifier = modifier
            .fillMaxHeight()
            .width(150.dp),
        verticalArrangement = Arrangement.Center,
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Text(
            text = "I am $name",
            style = MaterialTheme.typography.h6,
            textAlign = TextAlign.Center
        )
        Spacer(modifier = Modifier.height(32.dp))
        OutlinedButton(
            modifier = Modifier
                .wrapContentHeight()
                .fillMaxWidth(),
            onClick = onStart,
            enabled = !isOnShift && !hasTask
        ) {
            Text(
                textAlign = TextAlign.Center,
                text = "Start shift"
            )
        }
        Spacer(modifier = Modifier.height(8.dp))
        OutlinedButton(
            modifier = Modifier
                .wrapContentHeight()
                .fillMaxWidth(),
            onClick = onStop,
            enabled = isOnShift && !hasTask
        ) {
            Text(
                textAlign = TextAlign.Center,
                text = "Stop shift"
            )
        }
        if (isOnShift) {
            Spacer(modifier = Modifier.height(32.dp))
            Text(
                text = "Task UUID",
                style = MaterialTheme.typography.h6,
                textAlign = TextAlign.Center
            )
            Spacer(modifier = Modifier.height(32.dp))
            Text(
                text = taskId,
                style = MaterialTheme.typography.body1,
                textAlign = TextAlign.Center
            )
            if (hasTask) {
                Spacer(modifier = Modifier.height(32.dp))
                OutlinedButton(
                    modifier = Modifier
                        .wrapContentHeight()
                        .fillMaxWidth(),
                    onClick = onDone
                ) {
                    Text(
                        textAlign = TextAlign.Center,
                        text = "Confirm done"
                    )
                }
            }
        }
    }
}

@Preview(showBackground = true)
@Composable
fun DefaultPreview() {
    AndroidTheme {
        ShiftToggle(
            modifier = Modifier.size(400.dp),
            name = "Tom",
            taskId = "5d5d59c1-b023-4250-bc91-5ac9453e00d4",
            isOnShift = true, hasTask = true,
            onStart = {}, onStop = {}, onDone = {}
        )
    }
}
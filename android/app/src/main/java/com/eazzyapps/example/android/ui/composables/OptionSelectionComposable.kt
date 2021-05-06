package com.eazzyapps.example.android.ui

import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material.*
import androidx.compose.runtime.*
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.eazzyapps.example.android.IntakeActivity
import com.eazzyapps.example.android.IntakeViewModel
import com.eazzyapps.example.android.domain.SelectionOption
import com.eazzyapps.example.android.domain.SelectionOptions
import com.eazzyapps.example.android.get
import com.eazzyapps.example.android.ui.theme.typography

@Composable
fun OptionSelectionUI(
    sharedViewModel: IntakeViewModel = get(),
) {
    val selectedItem = rememberSaveable { mutableStateOf(NO_SELECTION) }

    val currentTask = sharedViewModel.currentTask.value

    val doOnConfirm: (Int) -> Unit = {
        sharedViewModel.sendInputData(currentTask.toResult(it))
    }

    val isError = currentTask.error != null

    val errorMsg = currentTask.error?.message ?: ""

    val optionList = (currentTask.payload as SelectionOptions).items

    Column(
        modifier = Modifier
            .padding(16.dp)
            .wrapContentHeight()
            .fillMaxWidth()
    ) {
        Text(
            text = "Select Bundle",
            style = typography.h6,
            fontWeight = FontWeight.Normal,
            modifier = Modifier.padding(start = 8.dp)
        )
        Spacer(modifier = Modifier.height(16.dp))
        LazyColumn(verticalArrangement = Arrangement.spacedBy(16.dp)) {
            items(optionList) { option ->
                OptionItemRow(
                    option = option,
                    selectedId = selectedItem.value,
                    onSelection = {
                        selectedItem.value = it
                    }
                )
            }
        }
        if (isError) {
            Spacer(modifier = Modifier.height(32.dp))
            Text(
                text = errorMsg,
                color = Color.Red,
                fontSize = 14.sp,
                modifier = Modifier
                    .align(Alignment.Start)
                    .padding(horizontal = 16.dp)
            )
        }
        Spacer(modifier = Modifier.height(32.dp))
        OutlinedButton(
            onClick = { doOnConfirm(selectedItem.value) },
            modifier = Modifier.align(Alignment.CenterHorizontally)
        ) {
            Text(text = "CONFIRM", style = typography.button)

        }

    }

}

@Composable
fun OptionItemRow(option: SelectionOption, onSelection: (Int) -> Unit, selectedId: Int) {
    Row(verticalAlignment = Alignment.CenterVertically) {
        RadioButton(
            selected = option.id == selectedId,
            onClick = { onSelection.invoke(option.id) },
            modifier = Modifier.padding(horizontal = 16.dp)
        )
        Text(text = option.text, style = typography.body1)
    }
}

const val NO_SELECTION = -1

fun getMockBundleList() = listOf(
    SelectionOption(
        id = 111,
        text = "12 x 1L"
    ),
    SelectionOption(
        id = 222,
        text = "24 x 1L"
    ),
    SelectionOption(
        id = 333,
        text = "36 x 0.5L"
    ),
    SelectionOption(
        id = 444,
        text = "6 x 1L"
    ),
    SelectionOption(
        id = 555,
        text = "3 x 1L"
    )

)
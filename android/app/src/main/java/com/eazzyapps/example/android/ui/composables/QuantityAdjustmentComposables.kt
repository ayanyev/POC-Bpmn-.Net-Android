package com.eazzyapps.example.android.ui.composables

import androidx.compose.foundation.layout.*
import androidx.compose.material.*
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.Remove
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import com.eazzyapps.example.android.get
import com.eazzyapps.example.android.getViewModel
import com.eazzyapps.example.android.ui.theme.typography
import com.eazzyapps.example.android.ui.viewmodel.IntakeViewModel
import com.eazzyapps.example.android.ui.viewmodel.QuantityAdjustmentViewModel

@ExperimentalMaterialApi
@Composable
fun QuantityAdjustmentDialog(
    sharedViewModel: IntakeViewModel = get()
) {

    val currentTask = sharedViewModel.currentTask.value

    val viewModel = getViewModel<QuantityAdjustmentViewModel>()

    val doOnConfirm: (Int) -> Unit = {
        sharedViewModel.sendInputData(currentTask.toResult(it))
    }

    val title = currentTask.label

    val quantity = viewModel.quantity.collectAsState()

    Column(
        modifier = Modifier
            .padding(16.dp)
            .wrapContentHeight()
            .fillMaxWidth()
    ) {
        Text(
            text = title,
            style = typography.h6,
            fontWeight = FontWeight.Normal
        )
        Spacer(modifier = Modifier.height(16.dp))
        Row(
            modifier = Modifier.fillMaxWidth()
        ) {
            Text(
                text = "Article Quantity",
                style = typography.subtitle1,
                fontWeight = FontWeight.Normal
            )
            Text(
                text = quantity.value.toString(),
                style = typography.subtitle1,
                fontWeight = FontWeight.SemiBold,
                modifier = Modifier.padding(start = 8.dp)
            )
        }
        Spacer(modifier = Modifier.height(16.dp))
        Row(
            modifier = Modifier.fillMaxWidth(),
            horizontalArrangement = Arrangement.SpaceBetween
        ) {
            Button(onClick = { viewModel.increaseQuantity(1) }) {
                Icon(
                    Icons.Filled.Add,
                    contentDescription = null
                )
                Text(text = "1", style = typography.button)
            }
            Button(
                onClick = { viewModel.increaseQuantity(10) },
                modifier = Modifier
                    .padding(start = 8.dp)
            ) {
                Icon(
                    Icons.Filled.Add,
                    contentDescription = null
                )
                Text(text = "10", style = typography.button)
            }
            Button(
                onClick = { viewModel.increaseQuantity(20) },
                modifier = Modifier
                    .padding(start = 8.dp)
            ) {
                Icon(
                    Icons.Filled.Add,
                    contentDescription = null
                )
                Text(text = "20", style = typography.button)
            }
        }
        Spacer(modifier = Modifier.height(8.dp))
        Row(
            modifier = Modifier.fillMaxWidth(),
            horizontalArrangement = Arrangement.SpaceBetween
        ) {
            Button(onClick = { viewModel.decreaseQuantity(1) }) {
                Icon(
                    Icons.Filled.Remove,
                    contentDescription = null
                )
                Text(text = "1", style = typography.button)
            }
            Button(
                onClick = { viewModel.decreaseQuantity(10) },
                modifier = Modifier
                    .padding(start = 8.dp)
            ) {
                Icon(
                    Icons.Filled.Remove,
                    contentDescription = null
                )
                Text(text = "10", style = typography.button)
            }
            Button(
                onClick = { viewModel.decreaseQuantity(20) },
                modifier = Modifier
                    .padding(start = 8.dp)
            ) {
                Icon(
                    Icons.Filled.Remove,
                    contentDescription = null
                )
                Text(text = "20", style = typography.button)
            }
        }
        Spacer(modifier = Modifier.height(32.dp))
        OutlinedButton(
            onClick = {
                doOnConfirm(quantity.value)
            },
            modifier = Modifier.align(Alignment.CenterHorizontally)
        ) {
            Text(text = "CONFIRM", style = typography.button)
        }
    }
}
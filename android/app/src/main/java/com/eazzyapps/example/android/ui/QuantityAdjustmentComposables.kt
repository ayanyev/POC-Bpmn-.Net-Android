package com.eazzyapps.example.android.ui

import androidx.compose.foundation.layout.*
import androidx.compose.material.*
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.Remove
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import com.eazzyapps.example.android.ui.theme.typography
import com.eazzyapps.example.android.ui.viewmodel.QuantityAdjustmentViewModel
import kotlinx.coroutines.launch

@ExperimentalMaterialApi
@Composable
fun QuantityAdjustmentUi(
    viewModel: QuantityAdjustmentViewModel,
    state: ModalBottomSheetState
) {
    val scope = rememberCoroutineScope()
    val quantity = viewModel.quantity.collectAsState()

    Column(
        modifier = Modifier
            .fillMaxWidth()
            .padding(32.dp), verticalArrangement = Arrangement.SpaceAround
    ) {
        Text(text = viewModel.articleName, style = typography.h6, fontWeight = FontWeight.SemiBold)
        Spacer(modifier = Modifier.height(8.dp))
        Row(modifier = Modifier.fillMaxWidth()) {
            Text(
                text = "Artikel anpassen",
                style = typography.subtitle1,
                fontWeight = FontWeight.Normal
            )
            Text(
                text = quantity.value.toString(),
                style = typography.subtitle1,
                fontWeight = FontWeight.Normal,
                modifier = Modifier.padding(start = 8.dp)
            )
        }
        Spacer(modifier = Modifier.height(16.dp))
        Row(modifier = Modifier.fillMaxWidth()) {
            Button(onClick = { viewModel.increaseQuantity(1) }) {
                Icon(
                    Icons.Filled.Add,
                    contentDescription = null,
                    modifier = Modifier.size(24.dp)
                )
            }
            Button(
                onClick = { viewModel.increaseQuantity(10) },
                modifier = Modifier.padding(start = 8.dp)
            ) {
                Icon(
                    Icons.Filled.Add,
                    contentDescription = null,
                    modifier = Modifier.size(24.dp)
                )
                Text(text = "10", style = typography.button)
            }
            Button(
                onClick = { viewModel.increaseQuantity(20) },
                modifier = Modifier.padding(horizontal = 8.dp)
            ) {
                Icon(
                    Icons.Filled.Add,
                    contentDescription = null,
                    modifier = Modifier.size(24.dp)
                )
                Text(text = "20", style = typography.button)
            }
        }
        Spacer(modifier = Modifier.height(8.dp))
        Row(modifier = Modifier.fillMaxWidth()) {
            Button(onClick = { viewModel.decreaseQuantity(1) }) {
                Icon(
                    Icons.Filled.Remove,
                    contentDescription = null,
                    modifier = Modifier.size(24.dp)
                )
            }
            Button(
                onClick = { viewModel.decreaseQuantity(10) },
                modifier = Modifier.padding(start = 8.dp)
            ) {
                Icon(
                    Icons.Filled.Remove,
                    contentDescription = null,
                    modifier = Modifier.size(24.dp)
                )
                Text(text = "10", style = typography.button)
            }
            Button(
                onClick = { viewModel.decreaseQuantity(20) },
                modifier = Modifier.padding(horizontal = 8.dp)
            ) {
                Icon(
                    Icons.Filled.Remove,
                    contentDescription = null,
                    modifier = Modifier.size(24.dp)
                )
                Text(text = "20", style = typography.button)
            }
        }
        Spacer(modifier = Modifier.height(16.dp))
        Row(
            modifier = Modifier
                .fillMaxWidth(), horizontalArrangement = Arrangement.End
        ) {
            TextButton(onClick = { scope.launch { state.hide() } }) {
                Text(text = "ABBRECHEN", style = typography.button)

            }
            TextButton(onClick = { viewModel.onConfirm() }) {
                Text(text = "AKTUALISIEREN", style = typography.button)

            }
        }
    }
}
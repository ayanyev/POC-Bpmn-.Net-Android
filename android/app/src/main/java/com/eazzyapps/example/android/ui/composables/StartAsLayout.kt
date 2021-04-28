package com.eazzyapps.example.android.ui.composables

import androidx.compose.foundation.layout.*
import androidx.compose.material.*
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowDropDown
import androidx.compose.material.icons.filled.ArrowDropUp
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp

@Preview
@Composable
fun StartAsLayoutPreview() {
    StartAsLayout(
        modifier = Modifier
            .fillMaxWidth()
            .wrapContentHeight(),
        items = listOf("Max", "Michael", "Uwe"),
        onSelected = {}
    )
}

@Composable
fun StartAsLayout(
    modifier: Modifier = Modifier,
    items: List<String>,
    selectionEnabled: Boolean = true,
    onSelected: (Int) -> Unit
) {

    var expanded by remember { mutableStateOf(false) }

    var selectionIndex by remember { mutableStateOf(-1) }

    var selectedValue by remember { mutableStateOf("Select name") }

    var isStarted by remember { mutableStateOf(false) }

    Box(
        modifier = Modifier
            .wrapContentSize(Alignment.Center)
            .then(modifier)
    ) {
        Column(Modifier.fillMaxWidth()) {
            Text(
                modifier = Modifier.fillMaxWidth(),
                text = "Start as",
                style = MaterialTheme.typography.h6,
                textAlign = TextAlign.Center
            )
            Spacer(modifier = Modifier.height(16.dp))
            OutlinedButton(
                modifier = Modifier.fillMaxWidth(),
                enabled = selectionEnabled,
                onClick = { expanded = true },
                contentPadding = PaddingValues(start = 16.dp)
            ) {
                Row(
                    verticalAlignment = Alignment.CenterVertically,
                    modifier = Modifier.wrapContentHeight()
                ) {
                    Text(modifier = Modifier.fillMaxWidth(0.8f), text = selectedValue)
                    IconButton(
                        modifier = Modifier.fillMaxWidth(),
                        enabled = selectionEnabled,
                        onClick = { expanded = !expanded }) {
                        Icon(
                            modifier = Modifier.size(24.dp),
                            imageVector = if (expanded) Icons.Default.ArrowDropUp else Icons.Default.ArrowDropDown,
                            contentDescription = ""
                        )
                    }
                }
            }
            DropdownMenu(
                modifier = Modifier.fillMaxWidth(),
                expanded = expanded,
                onDismissRequest = { expanded = false }
            ) {
                items.forEachIndexed { index, item ->
                    DropdownMenuItem(
                        modifier = Modifier.wrapContentSize(),
                        onClick = {
                            expanded = false
                            selectionIndex = index
                            selectedValue = item
                            onSelected(index)
                        }
                    ) {
                        Text(text = item, modifier = Modifier.requiredWidth(100.dp))
                    }
                }
            }
        }
    }
}
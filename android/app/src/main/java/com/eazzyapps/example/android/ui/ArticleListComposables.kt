package com.eazzyapps.example.android.ui

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.lazy.itemsIndexed
import androidx.compose.material.Divider
import androidx.compose.material.Icon
import androidx.compose.material.MaterialTheme
import androidx.compose.material.Text
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ExpandLess
import androidx.compose.material.icons.filled.ExpandMore
import androidx.compose.material.icons.filled.Minimize
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.alpha
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import com.eazzyapps.example.android.R
import com.eazzyapps.example.android.domain.Article
import com.eazzyapps.example.android.domain.ArticleBundle
import com.eazzyapps.example.android.domain.ArticleQuantity
import com.eazzyapps.example.android.ui.theme.grey
import com.eazzyapps.example.android.ui.theme.typography


@Composable
fun ArticleListComposable(articles: List<Article>) {

    LazyColumn(verticalArrangement = Arrangement.spacedBy(16.dp)) {
        val sorted =
            articles.sortedWith(compareBy<Article> { it.isCompleted() }.thenBy { !it.isSelected() })
        items(sorted) { article ->
            ArticleItemRow(article)
            Divider(color = grey.copy(alpha = 0.3f), thickness = 1.dp)
        }
    }
}

@Composable
fun ArticlesListHeader(count: Int, isCollapsed: Boolean) {
    Column(
        modifier = Modifier.height(64.dp).fillMaxWidth(),
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Text(
            text = "Articles: $count",
            modifier = Modifier.padding(top = 8.dp),
            style = MaterialTheme.typography.h6
        )
        Icon(
            modifier = Modifier.padding(bottom = 8.dp),
            imageVector = if (isCollapsed) Icons.Default.ExpandLess else Icons.Default.ExpandMore,
            contentDescription = null
        )
    }
}

@Composable
fun ArticleItemRow(article: Article) {
    Row(
        modifier = Modifier
            .fillMaxWidth()
            .alpha(if (article.isCompleted()) 0.4f else 1.0f)
            .background(if (article.isSelected()) Color.Blue.copy(alpha = 0.1f) else Color.White),
        verticalAlignment = Alignment.CenterVertically
    ) {
        Icon(
            modifier = Modifier.weight(0.2f),
            painter = painterResource(R.drawable.ic_bundle),
            contentDescription = null
        )
        Column(
            modifier = Modifier
                .weight(0.8f)
                .padding(horizontal = 16.dp)
        ) {
            Text(text = article.name, style = typography.body1)
            Spacer(Modifier.height(4.dp))
            Text(text = article.bundle.name, color = grey, style = typography.body2)
            Spacer(Modifier.height(8.dp))
            Row(
                modifier = Modifier.fillMaxWidth(),
                horizontalArrangement = Arrangement.SpaceBetween
            ) {
                Text(
                    text = "Expected Count : ${article.quantity.expected}",
                    style = typography.body1
                )
                Text(
                    text = "Processed Count : ${article.quantity.processed}",
                    style = typography.body1
                )
            }
            Spacer(Modifier.height(16.dp))
        }
    }
}

fun getMockArticleList() = listOf(
    Article(
        id = 111, noteId = "note1", name = "Pepsi Cola Maxx", gtin = "567.456.7.9",
        bundle = ArticleBundle(1, "6x1.5L"),
        quantity = ArticleQuantity(20, 0)
    ),
    Article(
        id = 222, noteId = "note1", name = "Pepsi Cola Maxx", gtin = "567.456.7.9",
        bundle = ArticleBundle(2, "12x1.0L"),
        quantity = ArticleQuantity(30, 0)
    ),
    Article(
        id = 333, noteId = "note1", name = "Gerolsteiner Naturell", gtin = "234.147.2.8",
        bundle = ArticleBundle(3, "12x1.0L Glass"),
        quantity = ArticleQuantity(20, 0)
    ),
    Article(
        id = 444, noteId = "note1", name = "Bauer Apfelsaft", gtin = "345.128.5.0",
        bundle = ArticleBundle(1, "6x1.5L"),
        quantity = ArticleQuantity(40, 0)
    ),
    Article(
        id = 555, noteId = "note1", name = "Gerolsteiner Mild", gtin = "195.247.2.7",
        bundle = ArticleBundle(3, "12x1.0L Glass"),
        quantity = ArticleQuantity(25, 0)
    ),
    Article(
        id = 666, noteId = "note1", name = "Bauer Orangensaft", gtin = "567.135.7.9",
        bundle = ArticleBundle(5, "24x0.2L"),
        quantity = ArticleQuantity(40, 0)
    )
)

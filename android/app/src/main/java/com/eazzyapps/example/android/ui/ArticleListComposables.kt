package com.eazzyapps.example.android.ui

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material.Divider
import androidx.compose.material.Icon
import androidx.compose.material.MaterialTheme
import androidx.compose.material.Text
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ExpandLess
import androidx.compose.material.icons.filled.ExpandMore
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import com.eazzyapps.example.android.R
import com.eazzyapps.example.android.domain.Article
import com.eazzyapps.example.android.domain.ArticleBundle
import com.eazzyapps.example.android.domain.ArticleQuantity
import com.eazzyapps.example.android.ui.theme.grey
import com.eazzyapps.example.android.ui.theme.typography

@Preview
@Composable
fun ArticleListPreview() {
    Box(
        modifier = Modifier.fillMaxSize(),
        contentAlignment = Alignment.Center
    ) {
        ArticleListComposable(
            listOf(
                article.apply { setState(Article.State.SELECTED) },
                article.copy(isSuspended = true),
                article.copy(isUnfinished = false),
                article,
                article
            )
        )
    }
}

private val article = Article(
    1, "1", "Pepsi Maxx", "567.456.7.9", ArticleQuantity(60, 0),
    ArticleBundle(1, "6x1.5L"), true, false
)

@Composable
fun ArticleListComposable(articles: List<Article>) {
    LazyColumn {
        val sorted =
            articles.sortedWith(compareBy<Article> { it.isCompleted || it.isSuspended }.thenBy { !it.isSelected })
        items(sorted) { article ->
            ArticleItemRow(article)
            Divider(color = grey.copy(alpha = 0.3f), thickness = 1.dp)
        }
    }
}

@Composable
fun ArticlesListHeader(count: Int, isCollapsed: Boolean) {
    Column(
        modifier = Modifier
            .height(64.dp)
            .fillMaxWidth(),
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

private fun getBackgroundColor(article: Article) = when {
    article.isSelected -> Color.Blue.copy(alpha = 0.1f)
    article.isSuspended -> Color.Red.copy(alpha = 0.1f)
    article.isCompleted -> Color.Gray.copy(alpha = 0.1f)
    else -> Color.White
}

@Composable
fun ArticleItemRow(article: Article) {
    Row(
        modifier = Modifier
            .fillMaxWidth()
            .background(getBackgroundColor(article)),
        verticalAlignment = Alignment.CenterVertically
    ) {
        Icon(
            painter = painterResource(R.drawable.ic_bundle),
            modifier = Modifier.fillMaxWidth(0.3f),
            contentDescription = null
        )
        Column(
            modifier = Modifier
                .fillMaxWidth()
                .padding(top = 16.dp, bottom = 16.dp, end = 16.dp)
        ) {
            Text(text = article.name, style = typography.body1)
            Spacer(Modifier.height(6.dp))
            Text(text = article.bundle.name, color = grey, style = typography.body2)
            Spacer(Modifier.height(6.dp))
            Text(
                modifier = Modifier.fillMaxWidth(),
                text = "Quantity : ${article.quantity.processed}/${article.quantity.expected}",
                style = typography.body1
            )
        }
    }
}

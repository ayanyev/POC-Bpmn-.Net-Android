package com.eazzyapps.example.android

data class Articles(
    val items: List<Article>
)

data class Article(
    val id: Int,
    val noteId: String,
    val name: String,
    val gtin: String,
    val quantity: ArticleQuantity,
    val bundle: ArticleBundle
)

data class ArticleQuantity(
    val expected: Int,
    val processed: Int
)

data class ArticleBundle(
    val id: Int,
    val name: String
)
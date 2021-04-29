package com.eazzyapps.example.android.domain

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
{
    enum class State { INITIAL, SELECTED }

    var currentState = State.INITIAL

    fun setState(state: State){
        currentState = state
    }

    fun isSelected() = currentState == State.SELECTED

    fun isCompleted() = quantity.processed >= quantity.expected

    override fun equals(other: Any?): Boolean {
        return (other as Article).let {
            it.id == id && it.noteId == noteId && it.gtin == gtin && it.name == name && it.quantity == quantity &&
            it.bundle.id == bundle.id && it.currentState == currentState
        }
    }

    override fun hashCode(): Int {
        var result = id
        result = 31 * result + noteId.hashCode()
        result = 31 * result + name.hashCode()
        result = 31 * result + gtin.hashCode()
        result = 31 * result + quantity.hashCode()
        result = 31 * result + bundle.hashCode()
        result = 31 * result + currentState.hashCode()
        return result
    }

}

data class ArticleQuantity(
    val expected: Int,
    val processed: Int
)

data class ArticleBundle(
    val id: Int,
    val name: String
)
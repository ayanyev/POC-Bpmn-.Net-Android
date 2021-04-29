<template>
  <b-container id="pickersList">
    <div v-for="article in articles" :key="article.id">
      <Article v-bind:article="article"/>
    </div>
  </b-container>
</template>

<script>
import Article from "@/components/Article";
export default {
  name: "ArticlesList",
  components: {
    Article
  },
  data() {
    return {
      articles: []
    }
  },
  methods: {
    addBarcodeScript() {
      const scriptTag = document.createElement("script");
      scriptTag.src = "https://cdn.jsdelivr.net/npm/jsbarcode@3.11.3/dist/JsBarcode.all.min.js";
      scriptTag.type = "text/javascript"
      document.head.appendChild(scriptTag);
    }
  },
  sockets: {
    DeliveryArticles(data) {
      this.articles = data
    }
  },
  created() {
    this.addBarcodeScript()
  },
  mounted() {
    this.$socket.invoke('GetDeliveryArticles', "note1")
  },
  updated() {
    if (!this.articles.empty) {
      window.JsBarcode(".barcode").init();
    }
  }
}
</script>

<style scoped>

</style>
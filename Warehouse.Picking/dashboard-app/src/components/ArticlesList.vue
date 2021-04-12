<template>
  <b-container id="pickersList">
    <b-table striped hover :items="articles"></b-table>
  </b-container>
</template>

<script>
export default {
  name: "ArticlesList",
  data() {
    return {
      articles: []
    }
  },
  methods: {
    addBarcodeScript() {
      let yourScript = document.createElement('script')
      yourScript.setAttribute('src', "https://cdn.jsdelivr.net/npm/jsbarcode@3.11.3/dist/JsBarcode.all.min.js")
      document.head.appendChild(yourScript)
    }
  },
  sockets: {
    DeliveryArticles(data) {
      console.log(data)
      this.articles = []
    }
  },
  mounted() {
    this.addBarcodeScript()
    this.$socket.invoke('GetDeliveryArticles', "noteId")
  },
  updated() {
    // JsBarcode(".barcode").init();
  }
}
</script>

<style scoped>

</style>
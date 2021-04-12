<template>
  <b-container id="pickersList">
    <b-table striped hover :items="articles"></b-table>
  </b-container>
</template>

<script src="https://cdn.jsdelivr.net/npm/jsbarcode@3.11.3/dist/barcodes/JsBarcode.ean-upc.min.js">
export default {
  name: "ArticlesList",
  data() {
    return {
      articles: []
    }
  },
  mounted() {
    this.$socket.invoke('GetDeliveryArticles')
    JsBarcode(".barcode").init();
  },
  sockets: {
    DeliveryArticles(data) {
      this.articles = []
    }
  }
}
</script>

<style scoped>

</style>
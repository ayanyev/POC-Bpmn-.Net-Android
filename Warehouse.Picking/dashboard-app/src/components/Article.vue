<template>
  <b-row>
    <b-col>
      <b-img :src="require('../assets/icon-stock.svg')"/>
    </b-col>
    <b-col>
      <b-row>Name: {{ article.name }}</b-row>
      <b-row>Quantity: {{ article.quantity }}</b-row>
      <b-row>Bundle: {{ article.bundle }}</b-row>
    </b-col>
    <b-col>
      <svg class="barcode"
           jsbarcode-textmargin="0"
           jsbarcode-height="64"
           :jsbarcode-value="article.gtin"/>
    </b-col>
  </b-row>
</template>
<script>
export default {
  name: "Article",
  props: ['article']
}
</script>

<style scoped>

</style>
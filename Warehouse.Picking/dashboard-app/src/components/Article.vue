<template>
  <b-row>
    <b-col align-self="center">
      <b-img :src="require('../assets/icon-stock.svg')"/>
    </b-col>
    <b-col align-self="center">
      <b-row>Name: {{ article.name }}</b-row>
      <b-row>Quantity: {{ article.quantity }}</b-row>
      <b-row>Bundle: {{ article.bundle }}</b-row>
    </b-col>
    <b-col>
      <b-row align-h="center">Article Gtin</b-row>
      <b-row>
        <svg class="barcode"
             jsbarcode-textmargin="0"
             jsbarcode-height="64"
             :jsbarcode-value="article.gtin"
             :jsbarcode-lineColor="article.color"/>
      </b-row>
    </b-col>
    <b-col>
      <b-row align-h="center">Location Gtin</b-row>
      <b-row>
        <svg class="barcode"
             jsbarcode-textmargin="0"
             jsbarcode-height="64"
             :jsbarcode-value="article.location"/>
      </b-row>
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
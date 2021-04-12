<template>
  <b-row>
    <b-col>
      <b-img src="./assets/icon-stock.svg"/>
    </b-col>
    <b-col>
      <b-row>{{ name }}</b-row>
      <b-row>{{ quantity }}</b-row>
      <b-row>{{ bundles }}</b-row>
    </b-col>
    <b-col>
      <svg class="barcode"
           jsbarcode-format="ean"
           :jsbarcode-value="gtin">
      </svg>
    </b-col>
  </b-row>
</template>
<script>
export default {
  name: "Article",
  props: ['pic', 'name', 'bundles', 'quantity', 'gtin']
}
</script>

<style scoped>

</style>
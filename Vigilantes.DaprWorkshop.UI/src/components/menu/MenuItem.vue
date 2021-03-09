<template>
  <el-col :span="6" style="margin:10px">
    <el-card :body-style="{ padding: '0px' }" shadow="hover">
      <img :src="menuItem.imageUrl" class="image" />
      <div style="padding: 14px;">
        <span>{{ menuItem.name }}</span>
        <div class="bottom clearfix">
          <div class="price">Price: {{ "$" + menuItem.price }}</div>
          <el-button type="text" class="button" @click="addToCart(menuItem)">{{this.buttonText}}</el-button>
        </div>
      </div>
    </el-card>
  </el-col>
</template>

<script>
export default {
  props: ["menuItem"],
  data() {
    return {
      buttonText: "Add To Cart"
    };
  },
  methods: {
    addToCart(menuItem) {
      this.$store.dispatch("addToCart", menuItem);
    }
  }
};
</script>

<style>

.price {
    font-size: 13px;
    color: #999;
  }

.bottom {
  margin-top: 13px;
  line-height: 12px;
}

.button {
  padding: 0;
  float: right;
}

.image {
  float: left;
  height: 150px;
  width: 150px;
  width: 100%;
  display: block;
}

.clearfix:before,
.clearfix:after {
  display: table;
  content: "";
}

.clearfix:after {
  clear: both;
}
</style>

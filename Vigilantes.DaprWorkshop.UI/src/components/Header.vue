<template>
  <el-menu
    :router="true"
    :default-active="this.$route.path"
    class="el-menu-demo"
    mode="horizontal"
  >
    <el-menu-item index="/">Cloud Coffee Company</el-menu-item>
    <el-submenu index="">
      <template slot="title">About</template>
      <el-menu-item index="/ourcompany">Cloud Coffee Company</el-menu-item>
    </el-submenu>
    <el-menu-item index="/menu"> Menu </el-menu-item>
    <el-menu-item index="/orders"> Orders </el-menu-item>
  </el-menu>
</template>

<script>
export default {
  data() {
    return {}
  }
}
</script>>

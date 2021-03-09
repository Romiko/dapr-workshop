<template>
  <el-container>
    <el-header height="100px" style="margin-top:50px">
      <img
        style="max-width:35%; height:auto; display:block; margin-left:auto; margin-right:auto"
        src="../assets/ourcompany-ourstory.png"
      />
    </el-header>
    <el-main style="margin:20px">
      <el-row :gutter="20">
        <el-col :span="12" :offset="6">
          <p>
            <strong> Cloud Coffee Co. is an independently owned, 'Bean to Bar' coffee company that’s taking the world by storm! </strong>
            <br /><br />
            We run an organic, fair-trade coffee shop in addition to providing an online ordering platform.
            Our goal is to provide customers with a highly personalized, omni-channel coffee experience.
            Our customer-centric mindset has led us to create an incentivized loyalty program that offers exciting rewards to repeat customers.
          </p>
          <hr />
          <p>
            Through the introduction of our loyalty program and the company’s continued investment in innovative beverage solutions, our loyal base of Cloud Coffee Co. consumers has been growing at an unprecedented pace over the past several months!
            With the onset of our exciting growth, we are currently modernizing our online ordering platform, <strong>a.k.a we are HIRING!</strong>
          </p>
  
        </el-col>
      </el-row>
    </el-main>

    <el-footer></el-footer>
  </el-container>
</template>

<script>
export default {};
</script>

<style scoped>
.el-row {
  margin-bottom: 20px;
  &:last-child {
    margin-bottom: 0;
  }
}
.el-col {
  border-radius: 4px;
}
</style>
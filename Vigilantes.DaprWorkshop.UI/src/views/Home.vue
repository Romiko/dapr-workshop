<template>
  <el-container class="homepage">
    <el-header>
      <el-row style="margin:20px">
        <el-col :span="8">
          <img
            style="float:right !important; margin-right: 10px; max-width:89%; height:auto;"
            src="../assets/welcome.png"
          />
        </el-col>
        <el-col :span="14">
          <img style="max-width:100%; height:auto;" src="../assets/homepage-name.png" />
        </el-col>
      </el-row>
    </el-header>
    <el-main>
      <el-row>
        <el-col :span="22">
          <img
            style="width:300px; max-width:100%; height:auto; display:block; margin-left:auto; margin-right:auto"
            src="../assets/coffee-cup.png"
          />
        </el-col>
      </el-row>
    </el-main>

    <el-footer>
      <el-row style="margin-top: 20px;">
        <el-col :span="8">
          <el-card class="box-card">
            <div slot="header" class="clearfix">
              <span>
                <strong>About</strong>
              </span>
            </div>        
            <div v-for="link in aboutUs" :key="link.path" class="link">
              <router-link :to="{path: link.path}">{{link.name}}</router-link>
            </div>         
          </el-card>
        </el-col>
        <el-col :span="8">
          <el-card class="box-card">
            <div slot="header" class="clearfix">
              <span>
                <strong>Dapr Resources</strong>
              </span>
            </div>
            <div v-for="link in resources" :key="link.url" class="link">
              <a :href=link.url target="_blank">{{link.name}}</a>
            </div>
          </el-card>
        </el-col>
      </el-row>
    </el-footer>
  </el-container>
</template>

<script>
export default {
  data(){
    return{
      aboutUs: [
        {path: "/ourcompany", name: "Cloud Coffee Co."}
        ],
      resources: [
        {url: "https://dapr.io", name: "Dapr homepage"}, 
        {url: "https://github.com/dapr/docs", name: "Dapr docs"}, 
        ]
    };
  }
}
</script>
<style scoped>


.text {
  font-size: 14px;
}

.link {
  padding: 18px 0;
}

.link:hover{
  font-size: 18px;
}

.homepage {
  background: #795548bd; 
  margin-top:10px;
}

.homepage:hover {
  background: #c5bebbd1; 
  margin-top:10px;
}

.box-card {
  width: 480px;
  max-width: 80%;
  margin: 10px;
  border: transparent;
  /* border-radius: 25px; */
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
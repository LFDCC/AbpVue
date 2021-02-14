<template>
  <div class="app-container">
   <el-card style="min-height:650px">
          <el-tabs v-model="activeTab" tab-position="left">
            <el-tab-pane
              :label="$t('AbpVue.AboutMe')"
              name="UserCard"
            >
               <user-card />
            </el-tab-pane>
            <el-tab-pane
              :label="$t('AbpAccount[\'ChangePassword\']')"
              name="ChangePassword"
            >
              <change-password />
            </el-tab-pane>
            <el-tab-pane
              :label="$t('AbpVue[\'PersonalData\']')"
              name="PersonalData"
            >
              <personal-data />
            </el-tab-pane>
             <el-tab-pane
              :label="$t('AbpVue[\'SecurityLogs\']')"
              name="SecurityLogs"
            >
              <security-logs />
            </el-tab-pane>
          </el-tabs>
        </el-card>
  </div>
</template>

<script>
import UserCard from "./components/UserCard";
import ChangePassword from "./components/ChangePassword";
import PersonalData from "./components/PersonalData";
import SecurityLogs from './components/MySecurityLogs'

export default {
  name: "Profile",
  components: { UserCard, ChangePassword, PersonalData,SecurityLogs },
  data() {
    return {
      activeTab: "UserCard",
    };
  },
};
</script>

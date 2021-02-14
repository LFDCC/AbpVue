<template>
  <div class="app-container">
    <div class="filter-container">
      <el-form
        ref="logQueryForm"
        label-position="right"
        label-width="120px"
        :model="queryForm"
      >
        <el-row>
          <el-col :span="6">
            <el-form-item
              prop="userName"
              :label="$t('LogManagement[\'UserName\']')"
            >
              <el-input
                clearable
                v-model="queryForm.userName"
                :placeholder="$t('AbpVue[\'Input\']', [])"
              />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item
              prop="applicationName"
              :label="$t('LogManagement[\'ApplicationName\']')"
            >
              <el-input
                clearable
                v-model="queryForm.applicationName"
                :placeholder="$t('AbpVue[\'Input\']', [])"
              />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item
              prop="actionName"
              :label="$t('LogManagement[\'Action\']')"
            >
              <el-input
                clearable
                v-model="queryForm.actionName"
                :placeholder="$t('AbpVue[\'Input\']', [])"
              />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item
              prop="correlationId"
              :label="$t('LogManagement[\'CorrelationId\']')"
            >
              <el-input
                clearable
                v-model="queryForm.correlationId"
                :placeholder="$t('AbpVue[\'Input\']', [])"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="6">
            <el-form-item
              prop="identity"
              :label="$t('LogManagement[\'Identity\']')"
            >
              <el-input
                clearable
                v-model="queryForm.identity"
                :placeholder="$t('AbpVue[\'Input\']', [])"
              />
            </el-form-item>
          </el-col>
          <el-col :span="9">
            <el-form-item :label="$t('LogManagement.DateTime')">
              <el-date-picker
                v-model="queryDateTime"
                type="datetimerange"
                align="right"
                unlink-panels
                value-format="yyyy-MM-dd HH:mm:ss"
                :picker-options="pickerOptions"
                :range-separator="$t('LogManagement[\'RangeSeparator\']')"
                :start-placeholder="$t('LogManagement[\'StartPlaceholder\']')"
                :end-placeholder="$t('LogManagement[\'EndPlaceholder\']')"
              />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-button
              type="reset"
              icon="el-icon-remove-outline"
              @click="resetQueryForm"
            >
              {{ $t("LogManagement.Reset") }}
            </el-button>
            <el-button
              type="primary"
              icon="el-icon-search"
              @click="handleSearch"
            >
              {{ $t("LogManagement.Search") }}
            </el-button>
          </el-col>
        </el-row>
      </el-form>
    </div>

    <div class="table-container">
      <el-table
        :key="tableKey"
        v-loading="listLoading"
        :data="list"
        border
        fit
        highlight-current-row
        style="width: 100%"
        @sort-change="sortChange"
        :default-sort="{ prop: 'creationTime', order: 'descending' }"
      >
        <el-table-column
          :label="$t('LogManagement[\'UserName\']')"
          prop="userName"
          align="center"
          min-width="130"
        >
          <template slot-scope="{ row }">
            <span>{{ row.userName }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LogManagement[\'TenantName\']')"
          prop="tenantName"
          align="center"
          min-width="130"
        >
          <template slot-scope="{ row }">
            <span>{{ row.tenantName }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LogManagement[\'CreationTime\']')"
          prop="creationTime"
          align="center"
          min-width="180"
          sortable="custom"
        >
          <template slot-scope="{ row }">
            <span>{{ row.creationTime }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LogManagement[\'Action\']')"
          prop="action"
          align="center"
          min-width="130"
        >
          <template slot-scope="{ row }">
            <span>{{ row.action }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LogManagement[\'ClientIpAddress\']')"
          prop="clientIpAddress"
          align="center"
          min-width="150"
        >
          <template slot-scope="{ row }">
            <span>{{ row.clientIpAddress }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LogManagement[\'ApplicationName\']')"
          prop="applicationName"
          align="center"
          min-width="130"
        >
          <template slot-scope="{ row }">
            <span>{{ row.applicationName }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LogManagement[\'Identity\']')"
          prop="identity"
          align="center"
          min-width="130"
        >
          <template slot-scope="{ row }">
            <span>{{ row.identity }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LogManagement[\'CorrelationId\']')"
          prop="correlationId"
          align="center"
          min-width="250"
        >
          <template slot-scope="{ row }">
            <span>{{ row.correlationId }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LogManagement[\'BrowserInfo\']')"
          prop="browserInfo"
          align="center"
          min-width="280"
        >
          <template slot-scope="{ row }">
            <span>{{ row.browserInfo }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LogManagement[\'Action\']')"
          prop="action"
          align="center"
          min-width="120"
          v-if="checkPermission('LogManagement.SecurityLogs.Delete')"
        >
          <template slot-scope="{ row }">
            <el-button
              v-if="checkPermission('LogManagement.SecurityLogs.Delete')"
              type="danger"
              @click="handleDelete(row, $index)"
            >
              {{ $t("AbpVue.Delete") }}
            </el-button>
          </template>
        </el-table-column>
      </el-table>
      <pagination
        v-show="total > 0"
        :total="total"
        :page.sync="queryForm.page"
        :limit.sync="queryForm.limit"
        @pagination="getList"
      />
    </div>
  </div>
</template>

<script>
import { getSecurityLogs, deleteSecurityLog } from "@/api/logging/securitylog";
import { pickerRangeWithHotKey } from "@/utils/picker";
import Pagination from "@/components/Pagination";
import baseListQuery, { checkPermission } from "@/utils/abp";
export default {
  name: "Securitylogging",
  components: { Pagination },
  data() {
    return {
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      queryDateTime: undefined,
      queryForm: Object.assign(
        {
          startTime: undefined,
          endTime: undefined,
          userName: undefined,
          applicationName: undefined,
          actionName: undefined,
          correlationId: undefined,
          identity: undefined,
        },
        baseListQuery
      ),
    };
  },
  computed: {
    pickerOptions: function () {
      return pickerRangeWithHotKey();
    },
  },
  created() {
    this.getList();
  },
  methods: {
    checkPermission,
    getList() {
      this.listLoading = true;
      if (this.queryDateTime) {
        this.queryForm.startTime = this.queryDateTime[0];
        this.queryForm.endTime = this.queryDateTime[1];
      } else {
        this.queryForm.startTime = undefined;
        this.queryForm.endTime = undefined;
      }
      getSecurityLogs(this.queryForm).then((response) => {
        this.list = response.items;
        this.total = response.totalCount;
        this.listLoading = false;
      });
    },
    resetQueryForm() {
      this.queryDateTime = undefined;
      this.queryForm = Object.assign(
        {
          startTime: undefined,
          endTime: undefined,
          userName: undefined,
          applicationName: undefined,
          actionName: undefined,
          correlationId: undefined,
          identity: undefined,
        },
        baseListQuery
      );
    },
    sortChange({ column, prop, order }) {
      if (order) {
        this.queryForm.sort = `${prop} ${order}`;
      } else {
        this.queryForm.sort = undefined;
      }
      this.handleSearch();
    },
    handleSearch() {
      this.queryForm.page = 1;
      this.getList();
    },
    handleDelete(row) {
      this.$confirm(
        this.$i18n.t("LogManagement['LogDeletionConfirmationMessage']", [
          row.userName,
        ]),
        this.$i18n.t("AbpVue['AreYouSure']"),
        {
          confirmButtonText: this.$i18n.t("AbpVue['Yes']"),
          cancelButtonText: this.$i18n.t("AbpVue['Cancel']"),
          type: "warning",
        }
      ).then(() => {
        deleteSecurityLog(row.id).then(() => {
          this.handleSearch();
          this.$notify({
            title: this.$i18n.t("AbpVue['Success']"),
            message: this.$i18n.t("AbpVue['SuccessMessage']"),
            type: "success",
            duration: 2000,
          });
        });
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.line {
  text-align: center;
}
.app-container {
  .api-block {
    height: auto;
    border: none;
    padding: 4px 0;
    margin: 2px;
  }
  .el-tag {
    margin: 2px;
  }
  .success {
    border-color: #49cc90;
    background: rgba(73, 204, 144, 0.1);
  }
  .danger {
    border-color: #f93e3e;
    background: rgba(249, 62, 62, 0.1);
  }
  .warning {
    border-color: #fca130;
    background: rgba(252, 161, 48, 0.1);
  }
}
</style>

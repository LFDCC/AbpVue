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
            <el-form-item prop="url" :label="$t('LogManagement[\'Url\']')">
              <el-input
                clearable
                v-model="queryForm.url"
                :placeholder="$t('AbpVue[\'Input\']', [])"
              />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item
              prop="httpMethod"
              :label="$t('LogManagement[\'HttpMethod\']')"
            >
              <el-select
                :placeholder="$t('AbpVue[\'Select\']', [])"
                v-model="queryForm.httpMethod"
                clearable
                style="width: 100%"
                @clear="queryForm.httpMethod = undefined"
              >
                <el-option label="GET" value="GET" />
                <el-option label="PUT" value="PUT" />
                <el-option label="POST" value="POST" />
                <el-option label="DELETE" value="DELETE" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item
              prop="ttpStatusCode"
              :label="$t('LogManagement[\'HttpStatusCode\']')"
            >
              <el-select
                :placeholder="$t('AbpVue[\'Select\']', [])"
                v-model="queryForm.httpStatusCode"
                clearable
                style="width: 100%"
                @clear="queryForm.httpStatusCode = undefined"
              >
                <el-option label="200" value="200" />
                <el-option label="204" value="204" />
                <el-option label="401" value="401" />
                <el-option label="403" value="403" />
                <el-option label="404" value="404" />
                <el-option label="500" value="500" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item :label="$t('LogManagement[\'ExecutionDuration\']')">
              <el-col :span="11" prop="minExecutionDuration">
                <el-input
                  clearable
                  v-model="queryForm.minExecutionDuration"
                  :placeholder="$t('AbpVue[\'Input\']', [])"
                />
              </el-col>
              <el-col class="line" :span="2">-</el-col>
              <el-col :span="11" prop="maxExecutionDuration">
                <el-input
                  clearable
                  v-model="queryForm.maxExecutionDuration"
                  :placeholder="$t('AbpVue[\'Input\']', [])"
                />
              </el-col>
            </el-form-item>
          </el-col>
        </el-row>
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
          <el-col :span="6">
            <el-form-item
              prop="hasException"
              :label="$t('LogManagement[\'HasException\']')"
            >
              <el-select
                :placeholder="$t('AbpVue[\'Select\']', [])"
                v-model="queryForm.hasException"
                clearable
                style="width: 100%"
                @clear="queryForm.hasException = undefined"
              >
                <el-option :label="$t('AbpVue.Yes')" value="true" />
                <el-option :label="$t('AbpVue.No')" value="false" />
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
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
        :default-sort="{ prop: 'executionTime', order: 'descending' }"
      >
        <el-table-column
          :label="$t('LogManagement[\'RequestInfo\']')"
          align="left"
          min-width="350"
        >
          <template slot-scope="{ row }">
            <el-tag :type="row.httpStatusCode | requestStatusCodeFilter">
              {{ row.httpStatusCode }}
            </el-tag>
            <el-tag :type="row.httpMethod | requestMethodFilter">
              {{ row.httpMethod }}
            </el-tag>
            <p
              class="api-block"
              :class="row.httpStatusCode | requestStatusCodeFilter"
            >
              {{ row.url }}
            </p>
          </template>
        </el-table-column>
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
          :label="$t('LogManagement[\'ExecutionTime\']')"
          prop="executionTime"
          align="center"
          min-width="180"
          sortable="custom"
        >
          <template slot-scope="{ row }">
            <span>{{ row.executionTime }}</span>
          </template>
        </el-table-column>
        <el-table-column
          :label="$t('LogManagement[\'ExecutionDuration\']')"
          prop="executionDuration"
          align="center"
          min-width="150"
          sortable="custom"
        >
          <template slot-scope="{ row }">
            <span>{{ row.executionDuration }}</span>
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
          :label="$t('LogManagement[\'Action\']')"
          prop="action"
          align="center"
          min-width="120"
        >
          <template slot-scope="{ row }">
            <el-dropdown @command="handleCommand" trigger="click">
              <el-button type="primary">
                {{ $t("LogManagement.Action") }}<i class="el-icon-arrow-down el-icon--right"></i>
              </el-button>
              <el-dropdown-menu slot="dropdown">
                <el-dropdown-item
                  icon="el-icon-document"
                  :command="{ type: 'info', data: row }"
                  > {{ $t("AbpVue.Detail") }}</el-dropdown-item
                >
                <el-dropdown-item
                v-if="checkPermission('LogManagement.AuditLogs.Delete')"
                  icon="el-icon-delete"
                  :command="{ type: 'dele', data: row }"
                  >{{ $t("AbpVue.Delete") }}</el-dropdown-item
                >
              </el-dropdown-menu>
            </el-dropdown>
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
      <audit-log-details ref="auditLogDetailsDialog" />
    </div>
  </div>
</template>

<script>
import { getAuditLogs,deleteAuditLog } from "@/api/logging/auditlog";
import { pickerRangeWithHotKey } from "@/utils/picker";
import Pagination from "@/components/Pagination";
import baseListQuery, { checkPermission } from "@/utils/abp";
import AuditLogDetails from "./details";
export default {
  name: "Auditlogging",
  components: { Pagination, AuditLogDetails },
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
          httpMethod: undefined,
          url: undefined,
          userName: undefined,
          applicationName: undefined,
          hasException: undefined,
          httpStatusCode: undefined,
          correlationId: undefined,
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
      getAuditLogs(this.queryForm).then((response) => {
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
          httpMethod: undefined,
          url: undefined,
          userName: undefined,
          applicationName: undefined,
          hasException: undefined,
          httpStatusCode: undefined,
          correlationId: undefined,
        },
        baseListQuery
      );
    },
    handleDetail(row) {
      this.$refs["auditLogDetailsDialog"].createLogInfo(row);
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
    handleCommand({ type, data }) {
      if (type == "info") {
        this.handleDetail(data);
      } else if (type == "dele") {
        this.handleDelete(data);
      }
    },
     handleDelete(row) {
      this.$confirm(
        this.$i18n.t("LogManagement['LogDeletionConfirmationMessage']", [
          row.userName
        ]),
        this.$i18n.t("AbpVue['AreYouSure']"),
        {
          confirmButtonText: this.$i18n.t("AbpVue['Yes']"),
          cancelButtonText: this.$i18n.t("AbpVue['Cancel']"),
          type: 'warning'
        }
      ).then(() => {
        deleteAuditLog(row.id).then(() => {
          this.handleSearch()
          this.$notify({
            title: this.$i18n.t("AbpVue['Success']"),
            message: this.$i18n.t("AbpVue['SuccessMessage']"),
            type: 'success',
            duration: 2000
          })
        })
      })
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

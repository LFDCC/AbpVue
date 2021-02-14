<template>
  <div class="audit-log-container">
    <el-dialog
      v-el-drag-dialog
      top="1%"
      :title="logData.url + '-' + $t('LogManagement[\'Detail\']')"
      :visible.sync="dialogVisible"
    >
      <el-tabs type="border-card" v-model="tabActive">
        <el-tab-pane
          class="cl-item"
          name="requestInfo"
          :label="$t('LogManagement[\'RequsetInfo\']')"
        >
          <table class="logInfo" style="width: 100%">
            <tbody>
              <tr>
                <th>{{ $t("LogManagement['HttpStatusCode']") }}</th>
                <td>
                  <el-tag
                    :type="logData.httpStatusCode | requestStatusCodeFilter"
                  >
                    {{ logData.httpStatusCode }}
                  </el-tag>
                </td>
              </tr>
              <tr>
                <th>{{ $t("LogManagement['HttpMethod']") }}</th>
                <td>
                  <el-tag :type="logData.httpMethod | requestMethodFilter">
                    {{ logData.httpMethod }}
                  </el-tag>
                </td>
              </tr>
              <tr>
                <th>{{ $t("LogManagement['Url']") }}</th>
                <td>
                  <p
                    class="api-block"
                    :class="logData.httpStatusCode | requestStatusCodeFilter"
                  >
                    {{ logData.url }}
                  </p>
                </td>
              </tr>
              <tr>
                <th>{{ $t("LogManagement['ClientIpAddress']") }}</th>
                <td>{{ logData.clientIpAddress }}</td>
              </tr>
              <tr>
                <th>{{ $t("LogManagement['ClientName']") }}</th>
                <td>{{ logData.clientName }}</td>
              </tr>
              <tr>
                <th>{{ $t("LogManagement['UserName']") }}</th>
                <td>{{ logData.userName }}</td>
              </tr>
              <tr>
                <th>{{ $t("LogManagement['ExecutionTime']") }}</th>
                <td>{{ logData.executionTime }}</td>
              </tr>
              <tr>
                <th>{{ $t("LogManagement['ExecutionDuration']") }}</th>
                <td>{{ logData.executionDuration }}ms</td>
              </tr>
              <tr>
                <th>{{ $t("LogManagement['BrowserInfo']") }}</th>
                <td>{{ logData.browserInfo }}</td>
              </tr>
              <tr>
                <th>{{ $t("LogManagement['Exceptions']") }}</th>
                <td>{{ logData.exceptions }}</td>
              </tr>
              <tr>
                <th>{{ $t("LogManagement['Comments']") }}</th>
                <td>{{ logData.comments }}</td>
              </tr>
              <tr>
                <th>{{ $t("LogManagement['ExtraProperties']") }}</th>
                <td>
                  <pre>{{ logData.extraProperties }}</pre>
                </td>
              </tr>
            </tbody>
          </table>
        </el-tab-pane>
        <el-tab-pane
          class="cl-item"
          name="requestParams"
          v-model="paramActive"
          :label="$t('LogManagement[\'Actions\']')"
        >
          <el-collapse
            :accordion="true"
            v-model="paramActive"
            v-if="logData.actions && logData.actions.length > 0"
            @change="paramChange"
          >
            <el-collapse-item
              v-for="action in logData.actions"
              :key="action.id + 'actionid'"
              :title="action.serviceName"
              :name="action.id + 'actionname'"
            >
              <table class="logInfo" style="width: 100%">
                <tbody>
                  <tr>
                    <th>{{ $t("LogManagement['MethodName']") }}</th>
                    <td>{{ action.methodName }}</td>
                  </tr>
                  <tr>
                    <th>{{ $t("LogManagement['ExecutionTime']") }}</th>
                    <td>{{ action.executionTime }}</td>
                  </tr>
                  <tr>
                    <th>{{ $t("LogManagement['ExecutionDuration']") }}</th>
                    <td>{{ action.executionDuration }}</td>
                  </tr>
                  <tr>
                    <th>{{ $t("LogManagement['Parameters']") }}</th>
                    <td>
                      <json-viewer
                        :value="
                          action.parameters ? JSON.parse(action.parameters) : {}
                        "
                        :expand-depth="5"
                        copyable
                        boxed
                        sort
                        @copied="onCopied"
                      >
                        <template slot="copy">
                          <el-button
                            type="primary"
                            round
                            size="small"
                            icon="el-icon-document"
                          >
                            {{ $t("AbpVue.Copy") }}
                          </el-button>
                        </template>
                      </json-viewer>
                    </td>
                  </tr>
                  <tr>
                    <th>{{ $t("LogManagement['ExtraProperties']") }}</th>
                    <td>{{ action.extraProperties }}</td>
                  </tr>
                </tbody>
              </table>
            </el-collapse-item>
          </el-collapse>
          <p v-else>
            {{ $t("AbpVue.Empty") }}
          </p>
        </el-tab-pane>
        <el-tab-pane
          class="cl-item"
          name="requestEntities"
          :label="$t('LogManagement[\'EntityChanges\']')"
        >
          <el-collapse
            :accordion="true"
            v-model="entityActive"
            v-if="logData.entityChanges && logData.entityChanges.length > 0"
            @change="entityChange"
          >
            <el-collapse-item
              v-for="entity in logData.entityChanges"
              :key="entity.id + 'entitykey'"
              :title="entity.entityTypeFullName"
              :name="entity.id + 'entityname'"
            >
              <table class="logInfo" style="width: 100%">
                <tbody>
                  <tr>
                    <th>{{ $t("LogManagement['ChangeType']") }}</th>
                    <td>{{ changeTypeFilter(entity.changeType) }}</td>
                  </tr>
                  <tr>
                    <th>{{ $t("LogManagement['ChangeTime']") }}</th>
                    <td>{{ entity.changeTime }}</td>
                  </tr>
                  <tr>
                    <th>{{ $t("LogManagement['ExtraProperties']") }}</th>
                    <td>{{ entity.extraProperties }}</td>
                  </tr>
                  <tr>
                    <th>{{ $t("LogManagement['Property']") }}</th>
                    <td>
                      <el-collapse
                        v-if="
                          entity.propertyChanges &&
                          entity.propertyChanges.length > 0
                        "
                        :key="entity.id + 'el'"
                      >
                        <el-collapse-item
                          v-for="prop in entity.propertyChanges"
                          :key="prop.id"
                          :title="prop.propertyName"
                          :name="prop.id"
                        >
                          <table class="logInfo" style="width: 100%">
                            <tbody>
                              <tr>
                                <th>
                                  {{ $t("LogManagement['PropertyType']") }}
                                </th>
                                <td>
                                  {{ prop.propertyTypeFullName }}
                                </td>
                              </tr>
                              <tr>
                                <th>
                                  {{ $t("LogManagement['OriginalValue']") }}
                                </th>
                                <td>{{ prop.originalValue }}</td>
                              </tr>
                              <tr>
                                <th>{{ $t("LogManagement['NewValue']") }}</th>
                                <td>{{ prop.newValue }}</td>
                              </tr>
                            </tbody>
                          </table>
                        </el-collapse-item>
                      </el-collapse>
                    </td>
                  </tr>
                </tbody>
              </table>
            </el-collapse-item>
          </el-collapse>
          <p v-else>
            {{ $t("AbpVue.Empty") }}
          </p>
        </el-tab-pane>
      </el-tabs>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogVisible = false">
          {{ $t("LogManagement.Close") }}
        </el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { getAuditLog } from "@/api/logging/auditlog";
export default {
  name: "AuditLogDetails",
  data() {
    return {
      tabActive: "requestInfo",
      dialogVisible: false,
      logData: {},
      paramActive: "",
      entityActive: "",
    };
  },
  computed: {
    changeTypeFilter() {
      return function (val) {
        switch (val) {
          case 0:
            return this.$t("AbpVue.Create");
            break;
          case 1:
            return this.$t("AbpVue.Update");
            break;
          case 2:
            return this.$t("AbpVue.Delete");
            break;
        }
      };
    },
  },
  methods: {
    getDetails() {
      getAuditLog(this.logData.id).then((response) => {
        this.logData = response;
        this.paramActive =
          this.logData.actions.length > 0 &&
          this.logData.actions[0].id + "actionname";
        this.entityActive =
          this.logData.entityChanges.length > 0 &&
          this.logData.entityChanges[0].id + "entityname";
      });
    },
    createLogInfo(row) {
      this.dialogVisible = true;
      this.logData = row;
      this.tabActive = "requestInfo";
      this.getDetails();
    },
    onCopied(copyEvent) {
      this.$message.success(this.$t("AbpVue.CopySuccess"));
    },
    entityChange(name) {
      this.entityActive = name;
    },
    paramChange(name) {
      this.paramActive = name;
    },
  },
};
</script>

<style lang="scss" scoped>
.audit-log-container {
  .logInfo {
    border-collapse: collapse;
    border-spacing: 2px;
    tr {
      border: 1px solid #f0f0f0;
      th {
        background-color: #fafafa;
        width: 120px;
      }
      th,
      td {
        text-align: left;
        text-overflow: ellipsis;
        vertical-align: middle;
        box-sizing: border-box;
        border-right: #f0f0f0;
        height: inherit;
        padding: 8px 16px;
      }
    }
  }
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
  .cl-item {
    height: 60vh;
    overflow: auto;
  }
}
</style>

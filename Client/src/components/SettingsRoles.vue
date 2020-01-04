<template>
  <v-flex
    v-if="canView"
    xs12 sm10 md8 lg6 xl4
  >
    <h2
      class="headline mb-3"
      v-t="'project.role.roles'"
    />
    <v-card
      flat
      class="ma-2 mb-5"
      :loading="loading"
    >
      <v-card-text
        class="grey--text"
        v-t="'project.role.description'"
      />
      <v-list>
        <v-list-item v-if="projectModule.getRoles.length === 0">
          <span v-t="'project.role.loading'" />
        </v-list-item>
        <v-list-item
          v-for="(role, index) in projectModule.getRoles"
          :key="index"
        >
          <v-list-item-content>
            <v-list-item-title v-text="role.name" />
            <v-list-item-subtitle>
              {{
                $tc('project.role.xParticipants', getPermitCount(role.id), {
                  count: getPermitCount(role.id)
                })
              }}
            </v-list-item-subtitle>
          </v-list-item-content>
          <v-list-item-action>
            <UpdateRoleDialog
              v-if="canEdit"
              :role="role"
              :canBeDeleted="getPermitCount(role.id) === 0"
            />
          </v-list-item-action>
        </v-list-item>
      </v-list>
      <v-card-actions v-if="!loading && canEdit">
        <v-spacer />
        <CreateRoleDialog />
      </v-card-actions>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import ProjectModule from '../store/projects'
import { Role } from '../models/definitions'
import CreateRoleDialog from '../components/dialogs/CreateRoleDialog.vue'
import UpdateRoleDialog from '../components/dialogs/UpdateRoleDialog.vue'
import PersonModule from '../store/persons'

@Component({
  components: {
    CreateRoleDialog,
    UpdateRoleDialog,
  }
})
export default class SettingsRoles extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private personModule: PersonModule = getModule(PersonModule, this.$store)

  private loading: boolean = true

  private get canView() {
    return this.personModule.getActiveRole?.rolesRead === true
  }

  private get canEdit() {
    return this.personModule.getActiveRole?.rolesWrite === true
  }

  private async created() {
    const projectId = this.$route.params.projectId
    await this.projectModule.loadRoles(projectId)
    await this.projectModule.loadParticipations(projectId)

    this.loading = false
  }

  private getPermitCount(roleId: string) {
    return this.projectModule.getParticipations.filter((x) => x.roleId === roleId).length
  }

}
</script>

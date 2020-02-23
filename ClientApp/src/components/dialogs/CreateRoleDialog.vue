<template>
  <v-dialog v-model="dialog" max-width="1000px">
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" text v-t="'project.role.create'" @click="opened" />
    </template>
    <v-card>
      <v-form v-model="valid" ref="form">
        <v-toolbar flat color="accent">
          <v-toolbar-title v-t="'project.role.create'" />
          <v-spacer />
          <v-btn icon class="mr-0" @click="showHelpText = !showHelpText">
            <v-icon v-if="showHelpText">help_outline</v-icon>
            <v-icon v-else>help</v-icon>
          </v-btn>
        </v-toolbar>
        <v-card-text v-if="showHelpText">
          <i class="subtitle-1" v-t="'project.role.createDescription'" />
        </v-card-text>
        <v-card-text class="pa-2">
          <NameControl :model="nameModel" translationPath="project.role.nameDescription" />
        </v-card-text>

        <v-divider />

        <v-card-text>
          <v-layout wrap>
            <PermissionControl
              :model="calendarModel"
              translationPath="project.role.calendar"
              :showDescription="showHelpText"
            />

            <PermissionControl
              :model="settingsModel"
              translationPath="project.role.settings"
              :showDescription="showHelpText"
            />

            <PermissionControl
              :model="rolesModel"
              translationPath="project.role.roles"
              :showDescription="showHelpText"
            />

            <PermissionControl
              :model="participantsModel"
              translationPath="project.role.participants"
              :showDescription="showHelpText"
            />

            <PermissionControl
              :model="knowledgeBaseModel"
              translationPath="project.role.knowledgeBase"
              :showDescription="showHelpText"
            />
          </v-layout>
        </v-card-text>

        <v-divider />

        <EligibilityControl
          v-for="(eligibility, index) in role.eligibilities"
          :key="index"
          :eligibility="eligibility"
          :showDescription="showHelpText"
        />

        <v-divider />

        <v-card-actions>
          <v-spacer />
          <v-btn text v-t="'core.cancel'" @click.stop="dialog = false" />
          <v-btn
            text
            type="submit"
            color="primary"
            v-t="'core.save'"
            :loading="loading"
            :disabled="!valid"
            @click.stop="save"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import ProjectModule from '../../store/projects'
import RoleModule from '../../store/roles'
import NameControl from '../controls/NameControl.vue'
import PermissionControl from '../controls/PermissionControl.vue'
import EligibilityControl from '../controls/EligibilityControl.vue'
import { Eligibility, Role } from '../../models'

@Component({
  components: {
    NameControl,
    PermissionControl,
    EligibilityControl,
  },
})
export default class CreateRoleDialog extends Vue {
  private projectModule = getModule(ProjectModule, this.$store)
  private roleModule = getModule(RoleModule, this.$store)

  private dialog = false
  private valid: any = null
  private loading = false
  private showHelpText = false

  private role = Role.create({
    eligibilities: [],
  })

  private nameModel = { value: '' }
  private calendarModel: { value: 'none' | 'read' | 'write' } = { value: 'read' }
  private settingsModel: { value: 'none' | 'read' | 'write' } = { value: 'read' }
  private rolesModel: { value: 'none' | 'read' | 'write' } = { value: 'read' }
  private participantsModel: { value: 'none' | 'read' | 'write' } = { value: 'read' }
  private knowledgeBaseModel: { value: 'none' | 'read' | 'write' } = { value: 'read' }

  private opened() {
    if (this.projectModule.getActiveProject) {
      this.role.eligibilities = this.projectModule.getActiveProject.categories.map((category) => {
        return Eligibility.create({
          category,
          categoryId: category.id,
          shiftsRead: true,
          shiftsWrite: false,
          isTeamCaptain: true,
          isSubstituteCaptain: false,
        })
      })
    }
  }

  private async save() {
    if (this.valid) {
      this.loading = true

      this.role.name = this.nameModel.value

      this.role.setPermissionModel('calendar', this.calendarModel.value)
      this.role.setPermissionModel('settings', this.settingsModel.value)
      this.role.setPermissionModel('roles', this.rolesModel.value)
      this.role.setPermissionModel('participants', this.participantsModel.value)
      this.role.setPermissionModel('knowledgeBase', this.knowledgeBaseModel.value)

      await this.roleModule.createRole(this.role)

      this.loading = false
      this.dialog = false
    }
  }
}
</script>

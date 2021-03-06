<template>
  <v-dialog v-model="deleteProjectDialog" max-width="600px">
    <template v-slot:activator="{ on }">
      <v-btn v-on="on" color="error" v-t="'project.delete'" @click="opened" />
    </template>
    <v-card>
      <v-form v-model="valid">
        <v-card-title>
          <span class="headline" v-t="'project.delete'" />
        </v-card-title>
        <v-card-text>
          <p class="subtitle-1" v-t="'project.deleteDescription'" />
          <p class="subtitle-1" v-t="'project.deleteHint'" />
          <v-text-field
            v-model="enteredName"
            :label="$t('core.name')"
            :rules="enteredNameRules"
            filled
            required
          />
          <p class="subtitle-1 text-uppercase error--text" v-t="'project.deleteWarning'" />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text @click.stop="deleteProjectDialog = false" v-t="'core.cancel'" />
          <v-btn
            :disabled="!valid"
            type="submit"
            text
            color="error"
            @click.prevent="deleteProject"
            v-t="'core.delete'"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import ProjectModule from '../../store/projects'
import { getModule } from 'vuex-module-decorators'
import { Project } from '../../models'
import i18n from '../../i18n'

@Component
export default class DeleteProjectDialog extends Vue {
  private projectModule = getModule(ProjectModule, this.$store)

  private valid = false
  private deleteProjectDialog = false
  private projectName: string = ''
  private enteredName: string = ''
  private get enteredNameRules() {
    return [
      (v: string) => !!v || i18n.t('core.fieldRequired'),
      (v: string) => v === this.projectName || i18n.t('project.nameMustEqual'),
    ]
  }

  private async opened() {
    const projectId = this.$route.params.projectId
    const project = this.projectModule.getProjects.filter((x: Project) => x.id === projectId)[0]
    if (project) {
      this.projectName = project.name
    }
  }

  private async deleteProject() {
    const projectId = this.$route.params.projectId

    await this.projectModule.deleteProject(projectId)

    this.deleteProjectDialog = false
    this.$router.push({ name: 'home' })
  }
}
</script>

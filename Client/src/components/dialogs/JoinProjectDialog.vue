<template>
  <v-dialog
    v-model="dialog"
    max-width="600px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        v-t="'project.join'"
      />
    </template>
    <v-card>
      <v-form v-model="form">
        <v-card-title>
          <span
            class="headline"
            v-t="'project.join'"
          />
        </v-card-title>
        <v-card-text>
          <p v-t="'project.joinDescription'" />
          <v-text-field
            v-model="projectId"
            :label="$t('core.id')"
            filled
            required
          />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn
            text
            @click.stop="dialog = false"
            v-t="'core.close'"
          />
          <v-btn
            :disabled="projectId == ''"
            text
            color="primary"
            @click.stop="joinProject"
            v-t="'project.joinSubmit'"
            type="submit"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { getModule } from 'vuex-module-decorators'
import { Vue, Component } from 'vue-property-decorator'
import i18n from '../../i18n'
import ProjectModule from '../../store/projects'
import { Topic } from '../../models/definitions'

@Component
export default class JoinProjectDialog extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private form: any = null
  private dialog: any = null
  private projectId: string = ''

  private async joinProject() {
    const projectId = this.$route.params.projectId

    await this.projectModule.joinProject(this.projectId)

    this.dialog = false
  }
}
</script>

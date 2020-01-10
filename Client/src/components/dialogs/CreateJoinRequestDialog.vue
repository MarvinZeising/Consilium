<template>
  <v-dialog
    v-model="dialog"
    max-width="600px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        v-t="'project.request.create'"
      />
    </template>
    <v-card>
      <v-form
        ref="form"
        v-model="form"
      >
        <v-card-title>
          <span
            class="headline"
            v-t="'project.request.create'"
          />
        </v-card-title>
        <v-card-text>
          <p v-t="'project.request.createDescription'" />
          <v-text-field
            v-model="projectId"
            :rules="projectRules"
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
            :loading="loading"
            text
            color="primary"
            @click.stop="joinProject"
            v-t="'project.request.createSubmit'"
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
import RequestModule from '../../store/requests'
import { Topic, Exceptions } from '../../models/definitions'

@Component
export default class CreateJoinRequestDialog extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)
  private requestModule: RequestModule = getModule(RequestModule, this.$store)

  private form: any = null
  private dialog: any = null
  private loading: boolean = false

  private projectId: string = ''
  private projectRules: any[] = []

  private throwValidationError(message: string) {
      const form: any = this.$refs.form
      const thisProjectId = this.projectId
      this.projectRules.push((v: string) => v !== thisProjectId || i18n.t(message))
      form.validate()
  }

  private async joinProject() {
    this.loading = true

    const response = await this.requestModule.createRequest(this.projectId)
    if (response === Exceptions.ProjectNotFound) {
      this.throwValidationError('project.notFound')
    } else if (response === Exceptions.RequestsNotAllowed) {
      this.throwValidationError('project.request.notAllowed')
    } else if (response) {
      this.throwValidationError('core.generalError')
    } else {
      this.dialog = false
    }

    this.loading = false
  }
}
</script>

<template>
  <v-dialog
    v-model="dialog"
    max-width="600px"
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        text
        v-t="'project.create'"
      />
    </template>
    <v-card>
      <v-form
        v-model="valid"
        ref="form"
      >
        <v-card-title>
          <span
            class="headline"
            v-t="'project.create'"
          />
        </v-card-title>
        <v-card-text>
            <p
              class="subtitle-1"
              v-t="'project.createDescription'"
            />
        </v-card-text>
        <v-card-text>
            <p v-t="'project.nameDescription'" />
            <v-text-field
              v-model="name"
              :label="$t('core.name')"
              :rules="nameRules"
              counter="40"
              filled
              required
            />
            <p v-t="'project.emailDescription'" />
            <v-text-field
              v-model="email"
              type="email"
              :label="$t('core.email')"
              :rules="emailRules"
              filled
              required
            />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn
            text
            @click.stop="dialog = false"
            v-t="'core.cancel'"
          />
          <v-btn
            text
            type="submit"
            color="primary"
            v-t="'core.save'"
            @click.stop="save"
            :loading="loading"
            :disabled="!valid"
          />
        </v-card-actions>
      </v-form>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import { VForm } from 'vuetify/lib'
import i18n from '../../i18n'
import AlertModule from '../../store/alerts'
import PersonModule from '../../store/persons'
import ProjectModule from '../../store/projects'
import { Exceptions } from '@/models/definitions'

@Component
export default class CreateProjectDialog extends Vue {
  private alertModule: AlertModule = getModule(AlertModule, this.$store)
  private personModule: PersonModule = getModule(PersonModule, this.$store)
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private dialog: any = false
  private valid: any = null
  private loading: boolean = false

  private name: string = ''
  private nameRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length <= 40 || i18n.t('core.fieldMax', { count: 40 }),
    (v: string) => v.length >= 3 || i18n.t('core.fieldMin', { count: 3 }),
    (v: string) => v.charAt(0) === v.charAt(0).toUpperCase() || i18n.t('core.fieldCamelCase')
  ]
  private email: string = ''
  private emailRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => /.+@.+\..+/.test(v) || i18n.t('core.emailInvalid')
  ]

  private async save() {
    this.loading = true

    const response: any = await this.projectModule.createProject({
      name: this.name,
      email: this.email,
    })
    if (response === Exceptions.ProjectNameUnique) {
      const form: any = this.$refs.form
      const thisName = this.name
      this.nameRules.push((v: string) => v !== thisName || i18n.t('project.nameUnique'))
      form.validate()
    } else if (response) {
      this.$router.push({
        name: 'settings',
        params: { projectId: response.id }
      })
    } else {
      this.alertModule.showAlert({
        message: i18n.t('core.generalError').toString(),
        color: 'error',
        timeout: 5000,
      })
    }

    this.loading = false
  }

}
</script>

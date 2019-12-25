<template>
  <v-container fluid>
    <h1
      class="headline mb-4"
      v-t="'project.create'"
    />

    <v-form
      ref="form"
      v-model="valid"
    >

      <v-flex xs12 sm10 md8 lg6 xl4>
        <v-stepper v-model="activeStep" vertical>

          <v-stepper-step
            :complete="activeStep > 1"
            step="1"
            v-t="'core.general'"
          />

          <v-stepper-content step="1">
            <p
              class="mt-4 grey--text text--darken-1"
              v-t="'project.nameDescription'"
            />
            <v-text-field
              v-model="name"
              :label="$t('core.name')"
              :rules="nameRules"
              counter="40"
              filled
              required
            />

            <p class="mt-4 grey--text text--darken-1">
              {{ $t('project.emailDescription1') }}
              <br>
              {{ $t('project.emailDescription2') }}
            </p>
            <v-text-field
              v-model="email"
              :label="$t('core.email')"
              type="email"
              :rules="emailRules"
              filled
              required
            />

            <v-btn
              :disabled="!valid"
              @click="validateStep"
              color="primary"
              v-t="'core.next'"
            />
            <v-btn
              text
              @click="goBack"
              v-t="'core.cancel'"
            />
          </v-stepper-content>

          <v-stepper-step
            :complete="activeStep > 2"
            step="2"
            v-t="'core.review'"
          />

          <v-stepper-content step="2">
            <p
              class="caption mb-0"
              v-t="'core.name'"
            />
            <p
              v-text="name"
              class="title"
            />

            <p
              class="caption mb-0"
              v-t="'core.email'"
            />
            <p
              v-text="email"
              class="title"
            />

            <v-btn
              @click="createProject"
              color="primary"
              v-t="'project.createSubmit'"
            />
            <v-btn
              text
              @click="activeStep = 1"
              v-t="'core.back'"
            />
          </v-stepper-content>

        </v-stepper>
      </v-flex>

    </v-form>

  </v-container>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { getModule } from 'vuex-module-decorators'
import i18n from '../../i18n'
import ProjectModule from '../../store/projects'

@Component
export default class CreateProject extends Vue {
  private projectModule: ProjectModule = getModule(ProjectModule, this.$store)

  private activeStep: number = 1
  private valid: boolean = false

  private name: string = ''
  private nameRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length <= 40 || i18n.t('core.fieldMax', { count: 40 }),
    (v: string) => v.length >= 3 || i18n.t('core.fieldMin', { count: 3 })
  ]

  private email: string = ''
  private emailRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => /.+@.+/.test(v) || i18n.t('core.emailInvalid')
  ]

  private goBack() {
    this.$router.back()
  }

  private validateStep() {
    const form: any = this.$refs.form

    if (form.validate()) {
      this.activeStep++
    }
  }

  private async createProject() {
    const form: any = this.$refs.form

    if (form.validate()) {
      const newProject = await this.projectModule.createProject({
        id: '',
        name: this.name,
        email: this.email
      })

      // TODO: change to project overview route (or first steps to configure the project)
      this.$router.push(`/project/${newProject.id}/calendar`)
    }
  }

}
</script>

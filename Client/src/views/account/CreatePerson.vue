<template>
  <v-container fluid>
    <h1
      class="headline mb-4"
      v-t="'person.create'"
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
            <p class="mt-4 grey--text text--darken-1">
              {{ $t('person.createDescription1') }}
              <br>
              {{ $t('person.createDescription2') }}
              <br>
              {{ $t('person.createDescription3') }}
            </p>
            <p class="mt-4 grey--text text--darken-1">
              {{ $t('person.description1') }}
              <br>
              {{ $t('person.description2') }}
            </p>
            <p
              class="mt-4 grey--text text--darken-1"
              v-t="'person.firstnameDescription'"
            />
            <v-text-field
              v-model="firstname"
              :label="$t('person.firstname')"
              :rules="nameRules"
              counter="40"
              filled
              required
            />

            <p
              class="mt-4 grey--text text--darken-1"
              v-t="'person.lastnameDescription'"
            />
            <v-text-field
              v-model="lastname"
              :label="$t('person.lastname')"
              :rules="nameRules"
              counter="40"
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
              v-t="'person.firstname'"
            />
            <p
              v-text="firstname"
              class="title"
            />

            <p
              class="caption mb-0"
              v-t="'person.lastname'"
            />
            <p
              v-text="lastname"
              class="title"
            />

            <v-btn
              @click="createPerson"
              color="primary"
              v-t="'person.create'"
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
import PersonModule from '../../store/persons'

@Component
export default class CreatePerson extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)

  private activeStep: number = 1
  private valid: boolean = false

  private firstname: string = ''
  private lastname: string = ''
  private nameRules: any[] = [
    (v: string) => !!v || i18n.t('core.fieldRequired'),
    (v: string) => v.length <= 40 || i18n.t('core.fieldMax', { count: 40 }),
    (v: string) => v.trim().length >= 2 || i18n.t('core.fieldMin', { count: 2 }),
    (v: string) => v.charAt(0) === v.charAt(0).toUpperCase() || i18n.t('core.fieldCamelCase')
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

  private async createPerson() {
    const form: any = this.$refs.form

    if (form.validate()) {
      await this.personModule.createPerson({
        firstname: this.firstname,
        lastname: this.lastname
      })

      // TODO: change to person overview route (or first steps to configure the person)
      this.$router.push({ name: 'personal' })
    }
  }

}
</script>

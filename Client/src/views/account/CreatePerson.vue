<template>
  <v-container class="createPerson">
    <h1 class="headline mb-4">Create a new Person</h1>

    <v-form
      ref="form"
      v-model="valid"
    >

      <v-flex xs12 sm10 md8 lg6>
        <v-stepper v-model="activeStep" vertical>

          <v-stepper-step
            :complete="activeStep > 1"
            step="1"
          >
            General
          </v-stepper-step>

          <v-stepper-content step="1">
            <p class="mt-4 grey--text text--darken-1">
              Why create a Person?
              <br>
              You already have a user. But a user is only for signing in.
              <br>
              To actually connect to a project and participate, you need a Person.
            </p>
            <p class="mt-4 grey--text text--darken-1">
              You can have multiple Persons connected to your user.
              <br>
              That way, you can manage the shifts for yourself and others (e.g. your spouse, parents, children, friends, etc.)
            </p>
            <p class="mt-4 grey--text text--darken-1">
              The first name of this Person.
            </p>
            <v-text-field
              v-model="firstname"
              label="Firstname"
              :rules="nameRules"
              counter="40"
              box
              required
            />

            <p class="mt-4 grey--text text--darken-1">
              The last name of this Person.
            </p>
            <v-text-field
              v-model="lastname"
              label="Lastname"
              :rules="nameRules"
              counter="40"
              box
              required
            />

            <v-btn
              :disabled="!valid"
              @click="validateStep"
              color="primary"
            >
              Continue
            </v-btn>
            <v-btn
              flat
              @click="goBack"
            >
              Cancel
            </v-btn>
          </v-stepper-content>

          <v-stepper-step
            :complete="activeStep > 2"
            step="2"
          >
            Review
          </v-stepper-step>

          <v-stepper-content step="2">
            <p class="caption mb-0">Firstname</p>
            <p
              v-text="firstname"
              class="title"
            />

            <p class="caption mb-0">Lastname</p>
            <p
              v-text="lastname"
              class="title"
            />

            <v-btn
              @click="createPerson"
              color="primary"
            >
              Create Person
            </v-btn>
            <v-btn
              flat
              @click="activeStep = 1"
            >
              Back
            </v-btn>
          </v-stepper-content>

        </v-stepper>
      </v-flex>

    </v-form>

  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import axios from 'axios'
import Component from 'vue-class-component'
import PersonModule from '@/store/modules/persons'
import { getModule } from 'vuex-module-decorators'

@Component
export default class CreatePerson extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)

  private activeStep: number = 1
  private valid: boolean = false

  private firstname: string = ''
  private lastname: string = ''
  private nameRules: any[] = [
    (v: string) => !!v || 'Name is required',
    (v: string) => v.length <= 40 || 'Name must be less than 40 characters',
    (v: string) => v.charAt(0) === v.charAt(0).toUpperCase() || 'Name must begin with an uppercase letter'
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
      const newPerson = await this.personModule.createPerson({
        firstname: this.firstname,
        lastname: this.lastname
      })

      // TODO: change to person overview route (or first steps to configure the person)
      this.$router.push({ name: 'home' })
    }
  }

}
</script>

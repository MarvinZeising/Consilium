<template>
  <v-container fluid fill-height>
    <v-layout align-center justify-center>
      <v-flex xs12 sm8 md6 lg4>
        <v-card
          class="mx-auto elevation-10"
          max-width="500"
        >
          <v-card-title class="title font-weight-regular justify-space-between">
            <span>{{ currentTitle }}</span>
            <v-avatar
              color="primary lighten-2"
              class="subheading white--text"
              size="24"
              v-text="step"
            ></v-avatar>
          </v-card-title>

          <v-window v-model="step">
            <v-window-item :value="1">
              <v-card-text>
                <v-text-field
                  v-model="email"
                  label="Email"
                />
                <span class="caption grey--text text--darken-1">
                  This is the email you will use to login to your Consilium account
                </span>
              </v-card-text>
            </v-window-item>

            <v-window-item :value="2">
              <v-card-text>
                <v-text-field
                  label="Password"
                  type="password"
                ></v-text-field>
                <v-text-field
                  label="Confirm Password"
                  type="password"
                ></v-text-field>
                <span class="caption grey--text text--darken-1">
                  Please enter a password for your account
                </span>
              </v-card-text>
            </v-window-item>

            <v-window-item :value="3">
              <div class="pa-3 text-xs-center">
                <v-img
                  class="mb-3"
                  contain
                  height="128"
                  src="https://cdn.vuetifyjs.com/images/logos/v.svg"
                ></v-img>
                <h3 class="title font-weight-light mb-2">Welcome to Vuetify</h3>
                <span class="caption grey--text">Thanks for signing up!</span>
              </div>
            </v-window-item>
          </v-window>

          <v-divider></v-divider>

          <v-card-actions>
            <v-btn
              :disabled="step === 1"
              flat
              @click="step--"
            >
              Back
            </v-btn>
            <v-spacer></v-spacer>
            <v-btn
              :disabled="step === 3"
              color="primary"
              depressed
              @click="next"
            >
              Next
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import axios from 'axios'
import Component from 'vue-class-component';

@Component
export default class SignUp extends Vue {
  private step: number = 1
  private email: string = ''

  private get currentTitle() {
    switch (this.step) {
      case 1: return 'Sign-up'
      case 2: return 'Create a password'
      default: return 'Account created'
    }
  }

  private next() {
    switch (this.step) {
      case 1: {
        axios.get(`/api/user?email=${this.email}`)
          .then((response) => {
            alert(response)
          })
          .catch((error) => {
            alert(error)
          })
      }
    }
  }

}
</script>

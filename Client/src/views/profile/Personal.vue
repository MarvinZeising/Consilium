<template>
  <v-container fluid>
    <v-layout wrap>

      <!--//* General Heading -->
      <v-flex xs12>
        <h2 class="headline mb-3">General</h2>
      </v-flex>
      <v-flex
        xs12 sm10 md8 lg6
        class="mb-5 pa-2"
      >
        <v-card flat>
          <v-card-text>
            <p class="caption mb-0 grey--text">
              Firstname
            </p>
            <p class="subheading">
              {{ firstname }}
            </p>

            <p class="caption mb-0 grey--text">
              Lastname
            </p>
            <p class="subheading mb-0">
              {{ lastname }}
            </p>
          </v-card-text>

          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn
              flat
              :to="{ name: 'home' }"
            >
              Edit
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-flex>

      <!--//* Photo Heading -->
      <v-flex xs12>
        <h2 class="headline mb-3">Photo</h2>
      </v-flex>
      <v-flex
        class="mb-5 pa-2 shrink"
      >
        <v-card flat>
          <v-card-text>
            <img
              v-if="photoUrl"
              :src="photoUrl"
              style="max-width:300px;"
            />
            <img
              v-if="!photoUrl"
              src="../../assets/person-default-image.jpg"
              style="max-width:200px;"
            />
          </v-card-text>

          <v-card-actions>
            <v-btn
              flat
              block
              :to="{ name: 'home' }"
            >
              Update Photo
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-flex>

    </v-layout>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from 'vue-class-component'
import { getModule } from 'vuex-module-decorators'
import { Person } from '@/models/definitions'
import { Watch, Prop } from 'vue-property-decorator'
import PersonModule from '@/store/modules/persons'

@Component
export default class Personal extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)

  private get firstname(): string {
    return this.personModule.getActivePerson.firstname
  }
  private get lastname(): string {
    return this.personModule.getActivePerson.lastname
  }
  private get photoUrl(): string {
    return this.personModule.getActivePerson.photoUrl
  }

  private async created() {
    await this.personModule.fetchPersons()
  }
}
</script>

<template>
  <v-container>
    <v-layout wrap>

      <!--//* General Heading -->
      <v-flex xs12 sm10 md8 lg6>
        <h2
          class="headline mb-3"
          v-t="'person.general'"
        />
        <v-card
          flat
          class="ma-2 mb-5"
        >
          <v-card-text>
            <p
              class="caption mb-0 grey--text"
              v-t="'person.firstname'"
            />
            <p class="subheading">
              {{ firstname }}
            </p>

            <p
              class="caption mb-0 grey--text"
              v-t="'person.lastname'"
            />
            <p class="subheading">
              {{ lastname }}
            </p>

            <p class="caption mb-0 grey--text">
              Gender
            </p>
            <p
              class="subheading"
              v-t="'person.gender'"
            />
          </v-card-text>

          <v-card-actions>
            <v-spacer />
            <v-btn
              flat
              :to="{ name: 'home' }"
              v-t="'core.edit'"
            />
          </v-card-actions>
        </v-card>
      </v-flex>

      <!--//* Photo Heading -->
      <v-flex xs12 sm10 md8 lg6>
        <h2
          class="headline mb-3"
          v-t="'person.photo'"
        />
        <v-card
          flat
          class="ma-2 mb-5"
        >
          <v-card-text>
            <img
              v-if="photoUrl"
              :src="photoUrl"
              style="max-width:300px;"
            />
            <img
              v-if="!photoUrl"
              src="../../assets/person.jpg"
              style="max-width:200px;"
            />
          </v-card-text>

          <v-card-actions>
            <v-spacer />
            <v-btn
              flat
              :to="{ name: 'home' }"
              v-t="'person.updatePhoto'"
            />
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
  private get gender(): string {
    return this.personModule.getActivePerson.gender
  }
  private get photoUrl(): string {
    return this.personModule.getActivePerson.photoUrl
  }
}
</script>

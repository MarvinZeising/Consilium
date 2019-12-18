<template>
  <v-flex xs12 sm10 md8 lg6>
    <h2
      class="headline mb-3"
      v-t="'person.persons'"
    />
    <v-card flat class="ma-2 mb-5">
      <v-card-text class="grey--text">
        {{ $t('person.personsDescription') }}
      </v-card-text>
      <v-list>
        <v-list-tile
          v-for="(person) in getPersons"
          :key="person.id"
        >
          <v-list-tile-content>
            <v-list-tile-title v-text="person.fullName()" />
          </v-list-tile-content>

          <v-list-tile-action>
            <v-btn
              icon
              class="ma-0"
              @click="editPerson(person.id)"
            >
              <v-icon color="grey">edit</v-icon>
            </v-btn>
          </v-list-tile-action>
        </v-list-tile>
      </v-list>
      <v-card-actions>
        <v-spacer />
        <v-btn
          flat
          :to="{ name: 'createPerson' }"
          v-t="'person.create'"
        />
      </v-card-actions>
    </v-card>
  </v-flex>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from 'vue-class-component'
import { getModule } from 'vuex-module-decorators'
import PersonModule from '../store/modules/persons'
import UserModule from '../store/modules/users'
import { Person } from '../models/definitions'

@Component({})
export default class MyPersons extends Vue {
  private personModule: PersonModule = getModule(PersonModule, this.$store)

  private get getPersons() {
    return this.personModule.getPersons
  }

  private editPerson(personId: string) {
    this.personModule.activatePerson(personId)
    this.$router.push({ name: 'personal' })
  }

}
</script>

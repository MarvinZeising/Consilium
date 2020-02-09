<template>

  <v-menu
    v-if="model"
    bottom
    right
  >
    <template v-slot:activator="{ on }">
      <v-btn
        v-on="on"
        outlined
      >
        <span v-t="'shift.' + model.type" />
        <v-icon right>arrow_drop_down</v-icon>
      </v-btn>
    </template>
    <v-list>
      <v-list-item @click="model.type = 'month'">
        <v-list-item-title v-t="'shift.month'" />
      </v-list-item>
      <v-list-item @click="model.type = 'week'">
        <v-list-item-title v-t="'shift.week'" />
      </v-list-item>
      <v-list-item @click="model.type = 'day'">
        <v-list-item-title v-t="'shift.day'" />
      </v-list-item>
    </v-list>
  </v-menu>

</template>

<script lang="ts">
import { Vue, Component, Prop } from 'vue-property-decorator'

@Component
export default class CalendarTypeControl extends Vue {

  @Prop(Object)
  private readonly model?: { type: string }

}
</script>

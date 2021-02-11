<template>
    <span
        v-if="value.status == 'modified' || value.status == 'deleted'"
        class="deleted"
        >{{ value.from }}</span>
    <span
        v-if="value.status == 'modified' || value.status == 'added'"
        class="added">{{ value.to }}</span>
    <span v-if="!value.status">{{ value }}</span>
</template>

<script src="@/components/Diff/Diff/Diff.js"></script>

<style lang="sass" scoped src="@/components/Diff/Diff/Diff.sass"></style>
